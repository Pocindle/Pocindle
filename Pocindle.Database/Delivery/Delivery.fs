module Pocindle.Database.Delivery


open System
open System.Net.Mail
open System.Threading.Tasks

open FSharp.Data.LiteralProviders
open FsToolkit.ErrorHandling
open FsToolkit.ErrorHandling.Operator.Result
open FSharp.UMX

open Pocindle.Convert.Domain
open Pocindle.Domain.SimpleTypes
open Pocindle.Database.Database
open Pocindle.Domain

let private deliveryStatusToData (deliveryStatus: DeliveryStatus) =
    match deliveryStatus with
    | HasNotDeliveryAddress -> 5, None
    | Failed msg -> 4, Some msg
    | DeliveryNotConfigured -> 3, None
    | NotStarted -> 2, None
    | InProcess -> 1, None
    | Done -> 0, None

let private deliveryStatusFromData deliverystatus deliveryfailedmessage =
    match deliverystatus with
    | 5 -> HasNotDeliveryAddress
    | 4 -> Failed deliveryfailedmessage
    | 3 -> DeliveryNotConfigured
    | 2 -> NotStarted
    | 1 -> InProcess
    | 0 -> Done
    | _ -> invalidArg "deliverystatus" $"{deliverystatus}"

let createDelivery
    connectionString
    (username: PocketUsername)
    (Article article)
    (EpubFile epub)
    (MobiFile mobi)
    (deliveryStatus: DeliveryStatus)
    (to': KindleEmailAddress)
    =
    taskResult {
        let ds, dm = deliveryStatusToData deliveryStatus

        let! ret =
            (querySingle<int64, _>
                connectionString
                TextFile.Delivery.``CreateDelivery.sql``.Text
                (Some
                    {| PocketUsername = PocketUsername.value username
                       ArticleUrl = string article
                       Epubfile = epub
                       MobiFile = mobi
                       DeliveryStatus = ds
                       DeliveryFailedMessage = dm |> Option.toObj
                       To = to' |> KindleEmailAddress.fromDomain |}))
            |> TaskResult.mapError DbException

        let! (ret1: DeliveryId) =
            match ret with
            | Some r -> Ok(%r)
            | None -> Error Empty

        return ret1
    }

type private DeliveryData =
    { deliveryid: int64
      userid: int64
      articleurl: string
      epubfile: string
      mobifile: string
      deliverystatus: int
      deliveryfailedmessage: string
      ``to``: string }

let getDeliveryById connectionString (deliveryId: DeliveryId) =
    taskResult {
        let! ret =
            (querySingle<DeliveryData, _>
                connectionString
                TextFile.Delivery.``GetDeliveryById.sql``.Text
                (Some {| DeliveryId = %deliveryId |}))
            |> TaskResult.mapError DbException

        let! ret1 =
            match ret with
            | Some r ->
                let delivery =
                    { Delivery.DeliveryId = %r.deliveryid
                      UserId = %r.userid
                      Article = Article(Uri(r.articleurl))
                      EpubFile = EpubFile r.epubfile
                      MobiFile = MobiFile r.mobifile
                      To = KindleEmailAddress.toDomain r.``to``
                      Status = deliveryStatusFromData r.deliverystatus r.deliveryfailedmessage }

                Ok delivery
            | None -> Error Empty

        return ret1
    }
