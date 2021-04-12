module internal Pocindle.Pocket.Retrieve.Implementation

open System
open System.Net.Http
open System.Text

open System.Text.Json
open FSharp.UMX
open FSharp.Control.Tasks
open FsToolkit.ErrorHandling
open FsToolkit.ErrorHandling.Operator.Result

open Pocindle.Pocket.Auth.SimpleTypes
open Pocindle.Pocket.Auth.PocketDto
open Pocindle.Pocket.Common.SimpleTypes
open Pocindle.Common.Serialization
open Pocindle.Pocket.Auth.PublicTypes
open Pocindle.Common.Serialization
open Pocindle.Pocket.Retrieve.PublicTypes
open Pocindle.Pocket.Retrieve.PocketDto

let retrieve : Retrieve =
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