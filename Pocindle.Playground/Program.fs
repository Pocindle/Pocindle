open System
open Pocindle.Convert.Library
open Pocindle.Convert.Domain

[<EntryPoint>]
let main argv =
    let res =
        convertToEpub
            Pandoc
            (Article
             <| Uri "https://habr.com/en/company/yandex/blog/547786/")
        |> Async.AwaitTask
        |> Async.RunSynchronously

    0
