module Pocindle.Convert.Implementation

open System
open System.IO
open FSharp.Control.Tasks
open SimpleExec

open Pocindle.Convert.Domain

let convertToEpub converter (Article article) =
    task {
        match converter with
        | Pandoc ->
            let filename = $"{Guid.NewGuid()}.epub"

            let args =
                $" %s{article.AbsoluteUri} -o %s{filename}"

            try
                do! Command.RunAsync("pandoc", args)
                return filename |> EpubFile |> Ok
            with ex -> return ex |> OtherError |> Error
        | _ -> return Error UndefinedTool
    }

let convertToMobi converter (EpubFile epub) =
    task {
        match converter with
        | Calible ->
            let mobi = epub.Replace(".epub", ".mobi")

            let args = $"%s{epub} %s{mobi}"

            try
                do! Command.RunAsync("ebook-convert", args)
                return mobi |> MobiFile |> Ok
            with ex -> return ex |> OtherError |> Error
        | _ -> return Error UndefinedTool
    }
