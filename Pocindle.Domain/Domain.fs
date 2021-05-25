namespace Pocindle.Domain

open Pocindle.Convert.Domain
open Pocindle.Domain.SimpleTypes

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
