module Pocindle.Pocket.Auth

open System
open System.Net.Http
open System.Net.Http
open System.Text
open System.Text.Json

open FSharp.UMX
open FSharp.Control.Tasks
open FsToolkit.ErrorHandling

open Pocindle.Pocket.SimpleTypes.Auth
open Pocindle.Pocket.PocketDto.Auth
open Pocindle.Pocket.Domain.Auth

let pocketSendRetrieve<'Request, 'Response> (request: 'Request) (uri: Uri) =
    taskResult {
        let! json =
            try
                Ok ^ JsonSerializer.Serialize(request)
            with ex -> Error ^ Exception ex

        let content =
            new StringContent(json, Encoding.UTF8, "application/json")

        use httpClient = new HttpClient()
        content.Headers.Add("X-Accept", "application/json")

        let! res1 =
            task {
                try
                    let! response = httpClient.PostAsync(uri, content)

                    let! res = response.Content.ReadAsStringAsync()
                    return Ok ^ JsonSerializer.Deserialize<'Response>(res)
                with ex -> return Error ^ Exception ex
            }

        return res1
    }

let obtainRequestToken (consumer_key: ConsumerKey) (redirect_uri: RedirectUri) (state: State) =
    taskResult {
        let req =
            { ObtainRequestTokenRequestDto.consumer_key = ConsumerKey.value consumer_key
              redirect_uri = RedirectUri.valueStr redirect_uri
              state = state |> Option.map (~%) |> Option.toObj }

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
            { AuthorizeRequestDto.consumer_key = ConsumerKey.value consumer_key
              code = RequestToken.value code }

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
