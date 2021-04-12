module internal Pocindle.Pocket.Retrieve.Implementation

open System
open System.Net.Http
open System.Text

open System.Text.Json
open FSharp.UMX
open FSharp.Control.Tasks
open FsToolkit.ErrorHandling
open FsToolkit.ErrorHandling.Operator.Result
open Oryx
open Oryx.SystemTextJson.ResponseReader

open Pocindle.Pocket.Auth.SimpleTypes
open Pocindle.Pocket.Auth.PocketDto
open Pocindle.Pocket.Common.SimpleTypes
open Pocindle.Common.Serialization
open Pocindle.Pocket.Auth.PublicTypes
open Pocindle.Common.Serialization
open Pocindle.Pocket.Retrieve.PublicTypes
open Pocindle.Pocket.Retrieve.PocketDto

let retrieve2 : Retrieve =
    fun consumerKey accessToken optionalParams ->
        let url = "https://getpocket.com/v3/get"

        let uri =
            $"%s{url}?access_token=%s{AccessToken.value accessToken}&consumer_key=%s{ConsumerKey.value consumerKey}"

        taskResult {
            use client = new HttpClient()
            let! i = client.GetAsync(uri)
            let! y = i.Content.ReadAsStringAsync()

            let! r =
                deserialize<RetrieveResponsePocketDto> y
                |> Result.mapError DeserializationError

            let! p =
                r
                |> RetrieveResponsePocketDto.toDomain
                |> Result.mapError ValidationError

            return p
        }

let retrieve : Retrieve =
    fun consumerKey accessToken optionalParams ->

        task {
            let url = "http://getpocket.com/v3/get"

            use client = new HttpClient()
            client.Timeout <- TimeSpan.FromMinutes 30.

            let query =
                [ ConsumerKey.toQuery consumerKey
                  AccessToken.toQuery accessToken ]
                @ (RetrieveOptionalParametersQuery.toQuery optionalParams)

            let ctx =
                HttpContext.defaultContext
                |> HttpContext.withHttpClient client
                |> HttpContext.withHeader (XAccept, ApplicationJson)

            try
                let! y =
                    GET
                    >=> withUrl url
                    >=> withQuery query
                    >=> withResponseType ResponseType.JsonValue
                    >=> fetch
                    >=> json<RetrieveResponsePocketDto> emptyOptions
                    |> runUnsafeAsync ctx

                let u = y
                return unimplemented ""
            with ex ->
                let y = ex
                raise ex

            // let r =
            //    RetrieveResponsePocketDto.toDomain y
            //    |> Result.mapError ValidationError

            return unimplemented ""
        }

let retrieve1 : Retrieve =
    fun consumerKey accessToken optionalParams ->
        let Url = "https://en.wikipedia.org/w/api.php"

        let options = JsonSerializerOptions()

        let query term =
            [ struct ("action", "opensearch")
              struct ("search", term) ]

        let request term =
            GET
            >=> withUrl Url
            >=> withQuery (query term)
            >=> fetch
            >=> json options

        let asyncMain _ =
            task {
                use client = new HttpClient()

                let ctx =
                    HttpContext.defaultContext
                    |> HttpContext.withHttpClient client

                let! result = request "F#" |> runAsync ctx
                printfn "Result: %A" result
            }

        asyncMain () |> ignore
        unimplemented ""
