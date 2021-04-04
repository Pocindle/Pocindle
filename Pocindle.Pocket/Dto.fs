module Pocindle.Pocket.Dto

open FSharp.Data
open Pocindle.Pocket.Domain
open Pocindle

[<Literal>]
let private ``pocket_retrieve.json`` =
    __SOURCE_DIRECTORY__ + "/pocket_retrieve.json"

type Retrieve = JsonProvider<``pocket_retrieve.json``>

type PocketItemDto = Retrieve.``937072498``

module PocketItem =
    let toDomain (a: PocketItemDto) =
        unimplemented ""

