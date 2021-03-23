module Pocindle.Domain

open System.Net.Mail
open System.Threading.Tasks

type Undefined = exn

type UserId = UserId of uint64

type User = { UserId: UserId }

type KindleEmailAddress = KindleEmailAddress of MailAddress

type Article = Article of Undefined

type Delivery =
    { UserId: UserId
      Data: Undefined
      To: KindleEmailAddress }
