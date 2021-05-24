namespace Pocindle.Domain

open System.Net.Mail
open System.Threading
open System.Threading.Tasks

open Pocindle.Convert.Domain
open Pocindle.Domain.SimpleTypes


type DeliveryConfig =
    { SmtpServer: SmtpServer
      From: MailAddress
      To: MailAddress
      Password: string }

type KindleEmailAddress =
    | HasKindleEmailAddress of MailAddress
    | NoneKindleEmailAddress

module KindleEmailAddress =
    let toDomain kmaString =
        match kmaString |> Option.ofObj with
        | Some m -> HasKindleEmailAddress(MailAddress(m))
        | None -> NoneKindleEmailAddress

    let fromDomain kma =
        match kma with
        | HasKindleEmailAddress mail -> string mail
        | NoneKindleEmailAddress -> null

type User =
    { UserId: UserId
      PocketUsername: PocketUsername
      DeliveryConfig: DeliveryConfig option }

type DeliveryStatus =
    | HasNotDeliveryAddress
    | Failed of string
    | DeliveryNotConfigured
    | NotStarted
    | InProcess
    | Done

type Delivery =
    { DeliveryId: DeliveryId
      UserId: UserId
      Article: Article
      EpubFile: EpubFile
      MobiFile: MobiFile
      To: KindleEmailAddress
      Status: DeliveryStatus }
