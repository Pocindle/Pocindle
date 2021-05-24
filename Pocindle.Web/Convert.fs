module Pocindle.Web.Convert

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
open Pocindle.Sending

let private articleToFiles articleUrl =
    taskResult {
        let article = Article(Uri(articleUrl))
        let! epub = convertWebToEpub Pandoc article
        let! mobi = convertEpubToMobi Calible epub
        return article, epub, mobi
    }

type ConvertRequestError =
    | DbError of DbError
    | ValidationError of string
    | DeliverySendingError of exn
    | ConvertError of ConvertError
    | NoneKindleEmailAddress'

let convert =
    (fun (articleUrl: string) next (ctx: HttpContext) ->
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

                    let! article, epub, mobi =
                        articleToFiles articleUrl
                        |> TaskResult.mapError ConvertError

                    let! user =
                        Users.getUserFromPocketUsername config.ConnectionString pocketUsername
                        |> TaskResult.mapError DbError

                    let! deliveryId =
                        Delivery.createDelivery
                            config.ConnectionString
                            pocketUsername
                            article
                            epub
                            mobi
                            NotStarted
                            user.KindleEmailAddress
                        |> TaskResult.mapError DbError


                    let! delivery =
                        Delivery.getDeliveryById config.ConnectionString deliveryId
                        |> TaskResult.mapError DbError

                    return user, delivery
                }

            match a with
            | Ok (user, delivery) ->
                let dto = DeliveryDto.fromDomain delivery
                return! json dto next ctx
            | Error error -> return raise500 error
        }),
    [ (StatusCodes.Status200OK, typeof<DeliveryDto>) ]
