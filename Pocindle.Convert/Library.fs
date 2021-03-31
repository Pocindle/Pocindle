module Pocindle.Convert.Library

open System.IO
open FSharp.Control.Tasks
open SimpleExec

open Pocindle.Convert.Domain

let convertToEpub: ConvertWebToEpub =
    fun converter (Article article) ->
        task {
            match converter with
            | Pandoc ->
                try
                    do! Command.RunAsync("pandoc", article.AbsoluteUri + " -o qwe1.epub")
                    return "qwe1.epub" |> EpubFile |> Ok
                with ex -> return ex |> OtherError |> Error
            | _ -> return Error NoPandoc
        }
