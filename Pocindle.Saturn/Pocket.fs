module Pocindle.Saturn.Pocket

open System.Net.Http
open System.Text.Json

open Saturn
open Giraffe.ResponseWriters
open FSharp.Control.Tasks

open Pocindle.Pocket
open Pocindle.Pocket.PocketDto.Retrieve
open Pocindle.Pocket.Dto.Retrieve

let pocketApi =
    router {
        getf
            "/retrieveAll/%s/%s"
            (fun (access_token, consumer_key) func ctx ->
                task {
                    let u =
                        $"https://getpocket.com/v3/get?access_token={access_token}&consumer_key={consumer_key}"

                    let httpClient = new HttpClient()
                    let! msg = httpClient.GetAsync(u)
                    let! g = msg.Content.ReadAsStringAsync()

                    let a =
                        JsonSerializer.Deserialize<PocketRetrieveRootPocketDto>(g)

                    let h =
                        a.list.Values
                        |> List.ofSeq
                        |> List.map PocketItemPocketDto.toDomain
                        |> List.map Result.get

                    let dtos =
                        h
                        |> List.map PocketItemDto.fromDomain
                        |> Array.ofList

                    let! r = json (dtos) func ctx
                    return r
                })
    }
