module Pocindle.Domain

open System.Net.Mail

type Undefined = exn

type UserId = UserId of uint64

type User = { UserId: UserId }

type KindleEmailAddress = KindleEmailAddress of MailAddress

type Article = Article of Undefined

type Delivery =
    { UserId: UserId
      Data: Undefined
      To: KindleEmailAddress }

type EpubFile = Undefined

type ConvertError =
    | NoPandoc
    | NoCalibre
    | Other of Undefined

type WebToEpubConverter = | Pandoc of Undefined
                          | Other of Undefined

type ConvertWebToEpub = WebToEpubConverter -> Article -> Async<Result<EpubFile, ConvertError>>
