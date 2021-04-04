module Pocindle.Convert.Library

open System.IO
open FSharp.Control.Tasks
open FsToolkit.ErrorHandling
open SimpleExec

open Pocindle.Convert.Domain

let convertToEpub : ConvertWebToEpub =
    fun converter (Article article) ->
        taskResult {
            match converter with
            | Pandoc ->
                try
                    do! Command.RunAsync("pandoc", article.AbsoluteUri + " -o qwe1.epub")
                    return "qwe1.epub" |> EpubFile
                with ex -> return! ex |> OtherError |> Error
            | _ -> return! Error NoPandoc
        }
