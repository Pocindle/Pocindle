module Pocindle.Web.Pocket

open System
open System.Security.Claims
open Microsoft.AspNetCore.Http

open FSharp.Control.Tasks
open FSharp.UMX
open Giraffe
open FsToolkit.ErrorHandling

open Pocindle.Domain.SimpleTypes
open Pocindle.Pocket.Retrieve
open Pocindle.Pocket.Retrieve.PublicTypes
open Pocindle.Database.Users


let retrieveAll =
    (fun next (ctx: HttpContext) ->
        task {
            let config = ctx.GetService<Config>()

            let! retrieve =
                taskResult {
                    let! pocketUsername =
                        ctx.User.FindFirst ClaimTypes.NameIdentifier
                        |> fun claim -> claim.Value
                        |> PocketUsername.create

                    let! accessToken =
                        getAccessTokenFromPocketUsername %config.ConnectionString pocketUsername
                        |> TaskResult.mapError string

                    let consumerKey = config.ConsumerKey

                    return Api.retrieve consumerKey accessToken
                }

            let! p =
                match retrieve with
                | Ok retrieve ->
                    let parameters =
                        { RetrieveOptionalParameters.empty with
                              Count =
                                  ctx.Request.Query.["count"]
                                  |> Core.queryStringValuesToOption
                                  |> Option.map int
                              Offset =
                                  ctx.Request.Query.["offset"]
                                  |> Core.queryStringValuesToOption
                                  |> Option.map int }

                    retrieve parameters
                | Error validationError -> raise500 validationError

            match p with
            | Ok response ->
                let dto =
                    Dto.PocketRetrieveDto.fromDomain response

                return! json dto next ctx
            | Error a -> return raise500 a
        }),
    [ (StatusCodes.Status200OK, typeof<Dto.PocketRetrieveDto>) ]
