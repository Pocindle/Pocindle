module Pocindle.Web.Delivery

open System
open System.ComponentModel.DataAnnotations
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
open Pocindle.Sending

type GetDeliveryError =
    | DbError of DbError
    | AccessDenied
    | ValidationError of string

let private getDiliveryFromCtx (deliveryId: int64) (ctx: HttpContext) =
    taskResult {
        let config = ctx.GetService<Config>()

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


let getDelivery =
    (fun (deliveryId: int64) next (ctx: HttpContext) ->
        task {
            let config = ctx.GetService<Config>()

            let! a = getDiliveryFromCtx deliveryId ctx

            match a with
            | Ok delivery ->
                let dto = DeliveryDto.fromDomain delivery
                return! json dto next ctx
            | Error AccessDenied -> return! RequestErrors.FORBIDDEN "Forbidden" next ctx
            | Error error -> return raise500 error
        }),
    [ (StatusCodes.Status200OK, typeof<DeliveryDto>)
      (StatusCodes.Status403Forbidden, typeof<string>) ]

type SendDeliveryError =
    | GetDeliveryError of GetDeliveryError
    | HasNotDeliveryAddress
    | DbError of DbError
    | ValidationError of string
    | DeliverySendingError of exn

let sendDelivery =
    (fun (deliveryId: int64) next (ctx: HttpContext) ->
        task {
            let config = ctx.GetService<Config>()

            let! newDelivery =
                taskResult {
                    let! pocketUsername =
                        ctx.User.FindFirst ClaimTypes.NameIdentifier
                        |> fun claim ->
                            claim.Value
                            |> PocketUsername.create
                            |> Result.mapError ValidationError

                    let! delivery =
                        getDiliveryFromCtx deliveryId ctx
                        |> TaskResult.mapError GetDeliveryError

                    let! user =
                        Users.getUserFromPocketUsername config.ConnectionString pocketUsername
                        |> TaskResult.mapError DbError

                    let! kindleEmailAddress =
                        match user.KindleEmailAddress with
                        | HasKindleEmailAddress to' -> Ok to'
                        | _ -> Error HasNotDeliveryAddress

                    let! s =
                        SendMobiDelivery.send
                            config.SmtpServer
                            config.SmtpSenderEmail
                            kindleEmailAddress
                            config.SmtpPassword
                            delivery.MobiFile
                        |> TaskResult.mapError DeliverySendingError

                    let! u =
                        Delivery.updateDeliveryStatus config.ConnectionString delivery.DeliveryId Done
                        |> TaskResult.mapError DbError

                    return!
                        Delivery.getDeliveryById config.ConnectionString delivery.DeliveryId
                        |> TaskResult.mapError DbError
                }

            match newDelivery with
            | Ok delivery ->
                let dto = DeliveryDto.fromDomain delivery
                return! json dto next ctx
            | Error (GetDeliveryError AccessDenied) -> return! RequestErrors.FORBIDDEN "Forbidden" next ctx
            | Error error -> return raise500 error
        }),
    [ (StatusCodes.Status200OK, typeof<DeliveryDto>)
      (StatusCodes.Status403Forbidden, typeof<string>) ]

type ServerEmailDto =
    { [<Required>]
      ServerEmailAddress: string }

let getServerEmailAddress =
    (fun next (ctx: HttpContext) ->
        task {
            let config = ctx.GetService<Config>()

            return! json { ServerEmailAddress = config.SmtpSenderEmail.Address } next ctx
        }),
    [ (StatusCodes.Status200OK, typeof<ServerEmailDto>) ]
