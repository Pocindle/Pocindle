open System
open System.IO
open System.Text.Json

open FSharp.UMX
open FsToolkit.ErrorHandling

open Pocindle.Convert.Domain
open Pocindle.Convert.Api

let zero x = 0

let min x = if x >= 0 then x - 1 else zero x

let rec map mathFunc =
    fun x y result ->
        if y = zero x then
            result
        else
            map mathFunc x (min y) (mathFunc result x)

let multiplication =
    map (fun result x -> result + x)

let power =
    map (fun result x -> multiplication result x 0)

let superpower x = power x x

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
