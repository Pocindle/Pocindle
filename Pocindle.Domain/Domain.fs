namespace Pocindle.Domain

open System.Net.Mail
open System.Threading.Tasks

open Pocindle.Domain.SimpleTypes


type KindleEmailAddress = KindleEmailAddress of MailAddress

type User =
    { UserId: UserId
      PocketUsername: PocketUsername
      KindleEmailAddress: KindleEmailAddress option }


type Article = Article of Undefined

type Delivery =
    { UserId: UserId
      Data: Undefined
      To: KindleEmailAddress }
