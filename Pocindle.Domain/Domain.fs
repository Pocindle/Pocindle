namespace Pocindle.Domain

open System.Net.Mail
open System.Threading.Tasks

open Pocindle.Domain.SimpleTypes


type User = { UserId: UserId }

type KindleEmailAddress = KindleEmailAddress of MailAddress

type Article = Article of Undefined

type Delivery =
    { UserId: UserId
      Data: Undefined
      To: KindleEmailAddress }
