open System
open System.IO
open System.Text.Json

open FSharp.UMX
open FsToolkit.ErrorHandling

open Pocindle.Convert.Domain
open Pocindle.Convert.Api

[<EntryPoint>]
let main args =

    let epub =
        convertWebToEpub
            Pandoc
            (Article(
                Uri(
                    "https://lexi-lambda.github.io/blog/2020/01/19/no-dynamic-type-systems-are-not-inherently-more-open/"
                )
            ))
        |> Async.AwaitTask
        |> Async.RunSynchronously

    match epub with
    | Ok epub ->
        let mobi =
            convertEpubToMobi Calible epub
            |> Async.AwaitTask
            |> Async.RunSynchronously

        printfn $"%A{mobi}"
    | Error error -> raise500 error

    0
