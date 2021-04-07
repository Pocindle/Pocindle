module Pocindle.Saturn.Pocket

open Saturn
open Giraffe.ResponseWriters
open FSharp.Control.Tasks
open FsToolkit.ErrorHandling

open Pocindle.Pocket.Retrieve
open Pocindle.Pocket.Retrieve.PublicTypes
open Pocindle.Pocket.Common.SimpleTypes

let pocketApi =
    router {
        getf
            "/retrieveAll/%s/%s"
            (fun (access_token, consumer_key) func ctx ->
                task {
                    let retrieve =
                        result {
                            let! accessToken = AccessToken.create access_token
                            let! consumerKey = ConsumerKey.create consumer_key

                            return Api.retrieve consumerKey accessToken
                        }

                    let! p =
                        match retrieve with
                        | Ok retrieve -> retrieve RetrieveOptionalParameters.empty
                        | Error validationError -> unimplemented ""

                    match p with
                    | Ok response ->
                        let dto =
                            Dto.PocketRetrieveDto.fromDomain response

                        return! (json dto func ctx)
                    | Error a -> return! (json a func ctx)
                })
    }
