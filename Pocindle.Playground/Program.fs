open System
open System.IO
open System.Text.Json

open FSharp.UMX
open Pocindle.Playground
open Pocindle.Pocket.PocketDto
open Pocindle.Pocket.Dto
open FsToolkit.ErrorHandling
open Pocindle.Common.Serialization

[<EntryPoint>]
let main args = 0
//    let s () =
//        let b = face {
//           let! a = member1 "123"
//           let a = a |> Result.get
//           let! c = member2<int> a
//
//           return c
//        }
//        let y = interpret<int> b
//        y
//
//    let u = s()
//
//    let abc a = a
//    let zxc = abc 2
//    0
//
//    //HJson.tryNJson<PocketItemDto> ()
//
//    let y =
//        result {
//
//            let! ct =
//                Pocindle.Pocket.SimpleTypes.Auth.ConsumerKey.create ""
//                |> Result.mapError Pocindle.Pocket.Domain.Auth.ParseError
//
//            let ru =
//                Pocindle.Pocket.SimpleTypes.Auth.RedirectString "https://pocindle.xyz/"
//
//            let! a =
//                (Pocindle.Pocket.Implementation.obtainRequestToken ct ru (Some(% "asda")))
//                |> Async.AwaitTask
//                |> Async.RunSynchronously
//
//            return a
//        }
//
//    printfn "%A" y
//
//    //    let q =
////        new StreamReader "pocket_retrieve_sample.json"
////
////    let q1 = q.ReadToEnd()
////
////    let root =
////        JsonSerializer.Deserialize<PocketRetrieveRootPocketDto>(q1)
////
////    let r =
////        root.list.Values
////        |> Seq.toList
////        |> List.map PocketItemPocketDto.toDomain
////        |> List.map Result.get
////
////    let d = r |> List.map PocketItemDto.fromDomain
////
////    printfn $"%A{d}"
//
//    0
