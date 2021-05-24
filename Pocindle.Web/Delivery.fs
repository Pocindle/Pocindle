module Pocindle.Web.Delivery

open System
open System.Security.Claims
open System.Threading.Tasks
open FsToolkit.ErrorHandling.Operator.TaskResult
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
open Pocindle.Database
open Pocindle.Convert.Api
open Pocindle.Convert.Domain

type GetDelivery =
    | DbError of DbError
    | AccessDenied
    | ValidationError of string

let getDelivery =
    (fun (deliveryId: int64) next (ctx: HttpContext) -> 
        task {
            let config = ctx.GetService<Config>()

            let! a =
                taskResult {
                    let! pocketUsername =
                        ctx.User.FindFirst ClaimTypes.NameIdentifier
                        |> fun claim ->
                            claim.Value
                            |> PocketUsername.create
                            |> Result.mapError ValidationError

                    let! delivery =
                        Delivery.getDeliveryById config.ConnectionString %deliveryId
                        |> TaskResult.mapError DbError

                    let! userId =
                        Users.getUserIdByPocketUsername config.ConnectionString pocketUsername
                        |> TaskResult.mapError DbError

                    if delivery.UserId = userId then
                        return! Ok ^ delivery
                    else
                        return! Error AccessDenied
                }

            match a with
            | Ok delivery ->
                let dto = DeliveryDto.fromDomain delivery
                return! json dto next ctx
            | Error AccessDenied -> return! RequestErrors.FORBIDDEN "Forbidden" next ctx
            | Error error -> return raise500 error
        }),
    [ (StatusCodes.Status200OK, typeof<DeliveryDto>)
      (StatusCodes.Status403Forbidden, typeof<string>) ]
