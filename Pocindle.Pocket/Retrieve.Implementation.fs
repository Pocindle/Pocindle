module internal Pocindle.Pocket.Retrieve.Implementation

open System
open System.Net.Http

open FsToolkit.ErrorHandling
open FsToolkit.ErrorHandling.Operator.Result

open Pocindle.Domain.SimpleTypes
open Pocindle.Common.Serialization
open Pocindle.Common
open Pocindle.Pocket.Retrieve.PublicTypes
open Pocindle.Pocket.Retrieve.PocketDto

let retrieve : Retrieve =
    fun consumerKey accessToken optionalParams ->
        let url = "https://getpocket.com/v3/get"

        let query =
            [ AccessToken.toQuery accessToken
              ConsumerKey.toQuery consumerKey ]
            @ (optionalParams
               |> RetrieveOptionalParametersQuery.toQuery)
            |> UriQuery.fromValueTuple

        let uri = UriBuilder(url)
        uri.Query <- query
        let uri = uri.Uri

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
