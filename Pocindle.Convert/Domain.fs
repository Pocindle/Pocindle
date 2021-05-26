module Pocindle.Convert.Domain

open System
open System.IO
open System.Net.Mail
open System.Threading.Tasks

type Article = Article of Uri

type EpubFile = EpubFile of string

type ConvertError =
    | NoPandoc
    | NoCalibre
    | UndefinedTool
    | OtherError of exn 

type WebToEpubConverter =
    | Pandoc
    | Other of Undefined

type ConvertWebToEpub = WebToEpubConverter -> Article -> Task<Result<EpubFile, ConvertError>>

type MobiFile = MobiFile of string

type EpubToMobiConverter =
    | Calible
    | Other of Undefined

type ConvertEpubToMobi = EpubToMobiConverter -> EpubFile -> Task<Result<MobiFile, ConvertError>>
