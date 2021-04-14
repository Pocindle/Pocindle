module Pocindle.Saturn.Pocket

open Saturn
open Giraffe.ResponseWriters
open FSharp.Control.Tasks
open FsToolkit.ErrorHandling

open Pocindle.Pocket.Retrieve
open Pocindle.Pocket.Retrieve.PublicTypes
open Pocindle.Pocket.Common.SimpleTypes
open Pocindle.Saturn

let pocketApi =
    router {
        getf
            "/retrieveAll/%s"
            (fun access_token func ctx ->
                task {
                    let retrieve =
                        result {
                            let! accessToken = AccessToken.create access_token
                            let consumerKey = (Controller.getConfig ctx).ConsumerKey

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
