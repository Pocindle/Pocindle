module Pocindle.Saturn.Pocket

open System.Security.Claims
open Microsoft.AspNetCore.Http

open FsToolkit.ErrorHandling
open FSharp.Control.Tasks
open Giraffe
open Giraffe.EndpointRouting.Routers
open Saturn
open Saturn.Endpoint

open Pocindle.Pocket.Retrieve
open Pocindle.Pocket.Retrieve.PublicTypes
open Pocindle.Pocket.Common.SimpleTypes
open Pocindle.Saturn

let retrieveAllByAccessToken =
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
    |> List.map (addMetadata (StatusCodes.Status200OK, typeof<Dto.PocketRetrieveDto>))

let retrieveByClaim =
    router {
        pipe_through (Auth.requireAuthentication JWT)

        get
            "/retrieveAll"
            (fun next ctx ->
                let email =
                    ctx.User.FindFirst ClaimTypes.NameIdentifier

                unimplemented "")
    }

let pocketApi =
    [ yield! retrieveAllByAccessToken
      yield! retrieveByClaim ]
