module Pocindle.Web.User

open System
open System.Security.Claims
open System.Threading.Tasks
open Microsoft.AspNetCore.Http

open FSharp.Control.Tasks
open FSharp.UMX
open Giraffe
open FsToolkit.ErrorHandling

open Pocindle.Domain
open Pocindle.Domain.Dto
open Pocindle.Domain.SimpleTypes
open Pocindle.Pocket.Retrieve
open Pocindle.Pocket.Retrieve.PublicTypes
open Pocindle.Database.Users
open Pocindle.Database

let setKindleEmailAddress =
    (fun (kma: string) (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let config = ctx.GetService<Config>()

            let! setKma =
                taskResult {
                    let! pocketUsername =
                        ctx.User.FindFirst ClaimTypes.NameIdentifier
                        |> fun claim -> claim.Value
                        |> PocketUsername.create

                    let! userId =
                        getUserIdByPocketUsername config.ConnectionString pocketUsername
                        |> TaskResult.mapError string

                    let kmad = KindleEmailAddress.toDomain kma

                    do!
                        setKindleMailAddressByUserId config.ConnectionString userId kmad
                        |> TaskResult.mapError string
                }

            match setKma with
            | Ok _ -> return! json () next ctx
            | Error dbError -> return raise500 dbError
        }),
    [ (StatusCodes.Status200OK, typeof<unit>) ]

let getUser =
    (fun next (ctx: HttpContext) ->
        task {
            let config = ctx.GetService<Config>()

            let! user =
                taskResult {
                    let! pocketUsername =
                        ctx.User.FindFirst ClaimTypes.NameIdentifier
                        |> fun claim -> claim.Value |> PocketUsername.create

                    return!
                        getUserFromPocketUsername config.ConnectionString pocketUsername
                        |> TaskResult.mapError string
                }

            match user with
            | Ok user -> return! json (UserDto.fromDomain user) next ctx
            | Error error -> return raise500 error
        }),
    [ (StatusCodes.Status200OK, typeof<UserDto>) ]
