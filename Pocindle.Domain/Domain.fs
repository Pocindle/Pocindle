namespace Pocindle.Domain

open System.Net.Mail
open System.Threading.Tasks

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


type Article = Article of Undefined

type Delivery =
    { UserId: UserId
      Data: Undefined
      To: KindleEmailAddress }
