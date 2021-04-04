open System

open FsToolkit.ErrorHandling

open Pocindle.Convert
open Pocindle.Convert.Library
open Pocindle.Convert.Domain
open Pocindle.Convert
open Pocindle.Convert.FreeSimpleExecCE

[<EntryPoint>]
let main _ =
    let sample =
        simpleExec {
            let y = result { return! Ok 1 }
            printfn $"%A{y}"
            return! FreeSimpleExec.simpleExecAsync "firefox"
        }

    let y = sample |> FreeSimpleExec.interpret

    let r = y.Result

    printfn $"%A{r}"

    //    let res =
//        convertToEpub
//            Pandoc
//            (Article
//             <| Uri "https://habr.com/en/company/yandex/blog/547786/")
//        |> Async.AwaitTask
//        |> Async.RunSynchronously

    0
