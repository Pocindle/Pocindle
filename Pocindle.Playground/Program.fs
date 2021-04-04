open System
open System.IO
open System.Text.Json

open Pocindle.Convert.Library
open Pocindle.Convert.Domain
open Pocindle.Pocket.Dto
open FsToolkit.ErrorHandling

[<EntryPoint>]
let main argv =
    let q =
        new StreamReader "pocket_retrieve_sample.json"

    let q1 = q.ReadToEnd()

    let root =
        JsonSerializer.Deserialize<PocketRetrieveRootDto>(q1)

    let r =
        root.list.Values
        |> Seq.toList
        |> List.map PocketItemDto.toDomain
        |> List.map
            (fun r ->
                match r with
                | Ok t -> t)

    printfn "%A" r

    0
