namespace Pocindle.Domain

open System.Net.Mail
open System.Threading
open System.Threading.Tasks

open Pocindle.Convert.Domain
open Pocindle.Domain.SimpleTypes


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
      KindleEmailAddress: KindleEmailAddress }


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
