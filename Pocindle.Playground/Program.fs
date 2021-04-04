open System
open System.IO
open System.Text.Json

open Pocindle.Convert.Library
open Pocindle.Convert.Domain
open Pocindle.Pocket.Dto

[<EntryPoint>]
let main argv =
    let q = new StreamReader "pocket_retrieve_sample.json"
    let q1 = q.ReadToEnd()
    
    //let o = JsonSerializerOptions(PropertyNameCaseInsensitive = true, Converters = [||] )
    
    let root = JsonSerializer.Deserialize<PocketRetrieveRootDto>(q1)
    
    //let list = root.list
    //RetrieveProvided.
    
    
    //let root1 = RetrieveProvided.Load(q)
     
    //let u =  root.List.JsonValue.
    
    //let s = PocketItemDto.toDomain
    
    //let w = s.List.``3279476375``.ResolvedTitle
    
//    let res =
//        convertToEpub
//            Pandoc
//            (Article
//             <| Uri "https://habr.com/en/company/yandex/blog/547786/")
//        |> Async.AwaitTask
//        |> Async.RunSynchronously

    
    0
