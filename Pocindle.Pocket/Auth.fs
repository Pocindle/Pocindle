module Pocindle.Pocket.Auth

open System
open System.Net.Http
open System.Net.Http
open System.Text
open System.Text.Json

open FSharp.UMX
open FsToolkit.ErrorHandling

open Pocindle.Pocket.SimpleTypes.Auth
open Pocindle.Pocket.PocketDto.Auth

let obtainRequestToken (consumer_key: ConsumerKey) (redirect_uri: RedirectUri) (state: State) =
    taskResult {
        let req =
            { ObtainRequestTokenRequestDto.consumer_key = ConsumerKey.value consumer_key
              redirect_uri = RedirectUri.valueStr redirect_uri
              state = state |> Option.map (~%) |> Option.toObj }

        let! json =
            (JsonSerializer.Serialize, req)
            ||> Result.throwableToResult
            |> Result.mapError string

        let content =
            new StringContent(json, Encoding.UTF8, "application/json")

        use httpClient = new HttpClient()
        content.Headers.Add("X-Accept", "application/json")

        let responseTask =
            httpClient.PostAsync(Uri("https://getpocket.com/v3/oauth/request", UriKind.Absolute), content)

        let! response =
            (id, responseTask)
            ||> TaskResult.throwableToResult
            |> TaskResult.mapError string

        let! res =
            (response.Content.ReadAsStringAsync, ())
            ||> TaskResult.throwableToResult
            |> TaskResult.mapError string

        let! res1 =
            ((JsonSerializer.Deserialize<ObtainRequestTokenResponseDto> : string -> _), res)
            ||> Result.throwableToResult
            |> Result.mapError string

        let stte : State =
            res1.state |> Option.ofObj |> Option.map (~%)

        let! rt = RequestToken.create res1.code
        return (rt, stte)
    }
