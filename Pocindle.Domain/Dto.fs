module Pocindle.Domain.Dto

open System
open System.ComponentModel.DataAnnotations
open System.Net.Mail

open FSharp.UMX
open FsToolkit.ErrorHandling
open Pocindle.Convert.Domain
open Pocindle.Domain.SimpleTypes


type UserDto =
    { [<Required>]
      UserId: int64
      [<Required>]
      PocketUsername: string
      KindleEmailAddress: string }

module UserDto =
    let fromDomain (user: User) =
        { UserDto.UserId = %user.UserId
          PocketUsername = user.PocketUsername |> PocketUsername.value
          KindleEmailAddress =
              match user.KindleEmailAddress with
              | HasKindleEmailAddress mail -> string mail
              | NoneKindleEmailAddress -> null }


type DeliveryDto =
    { [<Required>]
      DeliveryId: int64
      [<Required>]
      UserId: int64
      [<Required>]
      ArticleUrl: string
      [<Required>]
      EpubFile: string
      [<Required>]
      MobiFile: string
      To: string
      Status: Nullable<bool>
      StatusMessage: string }

module DeliveryDto =
    //    let toDomain a =
//        result {
//            try
//                let userId = a.UserId
//                let email = KindleEmailAddress(MailAddress(a.To))
//
//                return
//                    { Delivery.UserId = %userId
//                      Data = unimplemented ""
//                      To = email }
//            with ex -> return! Error ^ string ex
//        }

    let fromDomain (a: Delivery) =
        let deliveryId = %a.DeliveryId
        let userId = %a.UserId
        let (Article article) = a.Article
        let (EpubFile epub) = a.EpubFile
        let (MobiFile mobi) = a.MobiFile

        let email = KindleEmailAddress.fromDomain a.To

        let status =
            Option.toNullable
            ^ match a.Status with
              | HasNotDeliveryAddress -> None
              | Failed _ -> None
              | DeliveryNotConfigured -> None
              | NotStarted -> Some false
              | InProcess -> Some false
              | Done -> Some true

        let statusMessage =
            match a.Status with
            | HasNotDeliveryAddress -> "HasNotDeliveryAddress"
            | Failed msg -> msg
            | DeliveryNotConfigured -> "DeliveryNotConfigured"
            | NotStarted -> "NotStarted"
            | InProcess -> "InProcess"
            | Done -> null

        { DeliveryDto.DeliveryId = deliveryId
          UserId = userId
          ArticleUrl = string article
          EpubFile = epub
          MobiFile = mobi
          To = email
          Status = status
          StatusMessage = statusMessage }
