module Pocindle.Web.User

open System
open System.Security.Claims
open Microsoft.AspNetCore.Http

open FSharp.Control.Tasks
open FSharp.UMX
open Giraffe
open FsToolkit.ErrorHandling

open Pocindle.Domain
open Pocindle.Domain.SimpleTypes
open Pocindle.Pocket.Retrieve
open Pocindle.Pocket.Retrieve.PublicTypes
open Pocindle.Database.Users


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
            | Ok _ -> return json () next ctx
            | Error dbError -> return raise500 dbError
        }),
    [ (StatusCodes.Status200OK, typeof<unit>) ]
