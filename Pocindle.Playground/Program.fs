open System
open System.IO
open System.Text.Json

open Pocindle.Playground
open Pocindle.Pocket.PocketDto
open Pocindle.Pocket.Dto
open FsToolkit.ErrorHandling

[<EntryPoint>]
let main _ =
    HJson.tryNJson<PocketItemDto> ()


    //    let q =
//        new StreamReader "pocket_retrieve_sample.json"
//
//    let q1 = q.ReadToEnd()
//
//    let root =
//        JsonSerializer.Deserialize<PocketRetrieveRootPocketDto>(q1)
//
//    let r =
//        root.list.Values
//        |> Seq.toList
//        |> List.map PocketItemPocketDto.toDomain
//        |> List.map Result.get
//
//    let d = r |> List.map PocketItemDto.fromDomain
//
//    printfn $"%A{d}"

    0
