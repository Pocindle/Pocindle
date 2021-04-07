module Pocindle.Pocket.Auth.Implementation

open System
open System.Net.Http
open System.Text

open FSharp.UMX
open FsToolkit.ErrorHandling
open Oryx
open Oryx.SystemTextJson.ResponseReader

open Pocindle.Pocket.Auth.SimpleTypes
open Pocindle.Pocket.Auth.PocketDto
open Pocindle.Pocket.Common.SimpleTypes
open Pocindle.Common.Serialization
open Pocindle.Pocket.Auth.PublicTypes

let private pocketSendRetrieve<'RequestDto, 'ResponseDto> (request: 'RequestDto) (uri: Uri) =
    taskResult {
        let! json1 =
            serialize request
            |> Result.mapError SerializationError

        use client = new HttpClient()

        let ctx =
            HttpContext.defaultContext
            |> HttpContext.withHttpClient client

        return!
            POST
            >=> withUrl (string uri)
            >=> withContent (fun () -> new StringContent(json1, Encoding.UTF8, ApplicationJson) :> _)
            >=> withHeader XAccept ApplicationJson
            >=> fetch<'RequestDto>
            >=> json<'ResponseDto> emptyOptions
            |> runAsync ctx
            |> TaskResult.mapError FetchException
    }

let obtainRequestToken : ObtainRequestToken =
    fun (consumer_key: ConsumerKey) (redirect_uri: RedirectUri) (state: State) ->
        taskResult {
            let req =
                ObtainRequestTokenRequestDto.fromDomain consumer_key redirect_uri state

            let! res1 =
                pocketSendRetrieve<ObtainRequestTokenRequestDto, ObtainRequestTokenResponseDto>
                    req
                    (Uri("https://getpocket.com/v3/oauth/request", UriKind.Absolute))

            let stte : State =
                res1.state |> Option.ofObj |> Option.map (~%)

            let! rt =
                RequestToken.create res1.code
                |> Result.mapError ParseError

            return (rt, stte)
        }

let authorize (consumer_key: ConsumerKey) (code: RequestToken) =
    taskResult {
        let req =
            AuthorizeRequestDto.fromDomain consumer_key code

        let! res1 =
            pocketSendRetrieve<AuthorizeRequestDto, AuthorizeResponseDto>
                req
                (Uri("https://getpocket.com/v3/oauth/authorize", UriKind.Absolute))

        let! rt =
            AccessToken.create res1.access_token
            |> Result.mapError ParseError

        let! user =
            Username.create res1.username
            |> Result.mapError ParseError

        return (rt, user)
    }
