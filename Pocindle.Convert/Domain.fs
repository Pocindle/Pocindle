module Pocindle.Convert.Domain

open System
open System.IO
open System.Net.Mail
open System.Threading.Tasks

type Undefined = exn

type Article = Article of Uri

type EpubFile = EpubFile of string

type ConvertError =
    | NoPandoc
    | NoCalibre
    | OtherError of Undefined

type WebToEpubConverter = | Pandoc 
                          | Other of Undefined

type ConvertWebToEpub = WebToEpubConverter -> Article -> Task<Result<EpubFile, ConvertError>>
