module Pocindle.Saturn.Pocket

open System
open System.Security.Claims
open Microsoft.AspNetCore.Http

open FsToolkit.ErrorHandling
open FSharp.Control.Tasks
open Giraffe
open Giraffe.EndpointRouting.Routers
open Pocindle.Domain.SimpleTypes
open Saturn
open Saturn.Endpoint

open Pocindle.Pocket.Retrieve
open Pocindle.Pocket.Retrieve.PublicTypes
open Pocindle.Pocket.Common.SimpleTypes
open Pocindle.Saturn
open Pocindle.Database.Users

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
                task {
                    let! r =
                        taskResult {
                            let email =
                                ctx.User.FindFirst ClaimTypes.NameIdentifier

                            let! username =
                                PocketUsername.create email.Value
                                |> Result.mapError Exception

                            let! a =
                                getAccessTokenFromPocketUsername (Controller.getConfig ctx).ConnectionString username

                            let accessToken =
                                AccessToken.create (a |> Option.get) |> Result.get

                            let consumerKey = (Controller.getConfig ctx).ConsumerKey

                            let! a =
                                Api.retrieve consumerKey accessToken RetrieveOptionalParameters.empty
                                |> TaskResult.mapError (fun _ -> Exception())

                            let dto = Dto.PocketRetrieveDto.fromDomain a

                            return! (json dto next ctx)
                        }
                    return
                })
    }

let pocketApi =
    [ yield! retrieveAllByAccessToken
      yield! retrieveByClaim ]
