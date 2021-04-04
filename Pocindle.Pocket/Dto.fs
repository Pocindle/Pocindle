module Pocindle.Pocket.Dto

open System
open System.Collections.Generic
open FSharp.Data

open FsToolkit.ErrorHandling
open FSharp.UMX

open Pocindle.Pocket.Domain
open Pocindle
open Pocindle.Pocket.SimpleTypes

[<Literal>]
let private ``pocket_retrieve.json`` =
    __SOURCE_DIRECTORY__ + "/pocket_retrieve.json"

type RetrieveProvided = JsonProvider<``pocket_retrieve.json``>

type PocketItemDto = RetrieveProvided.``937072498``

type PocketRetrieveRootDto =
    { status: int
      complete: int
      list: IDictionary<string, Object>
      error: Object
      since: int64 }

module PocketItemDto =
    let toDomain (a: PocketItemDto) =
        result {
            let! givenUrl =
                match Uri.TryCreate(a.GivenUrl, UriKind.Absolute) with
                | true, b -> Ok b
                | _ -> Error "Invalid given_url"

            let! resolvedUrl =
                match Uri.TryCreate(a.ResolvedUrl, UriKind.Absolute) with
                | true, b -> Ok b
                | _ -> Error "Invalid resolved_url"

            let! ampUrl =
                match Uri.TryCreate(a.AmpUrl, UriKind.Absolute) with
                | true, b -> Ok(Some b)
                | _ when a.ResolvedUrl = "" -> Ok None
                | _ -> Error "Invalid amp_url"

            let! favorite =
                match a.Favorite with
                | 0 -> Ok NotFavorite
                | 1 -> Ok Favorite
                | _ -> Error "Invalid favorite "

            let! status =
                match a.Status with
                | 0 -> Ok Normal
                | 1 -> Ok Archived
                | 2 -> Ok ShouldBeDeleted
                | _ -> Error "Invalid status"

            let! isArticle =
                match a.IsArticle with
                | 0 -> Ok false
                | 1 -> Ok true
                | _ -> Error "Invalid is_article"

            let y =
                { ItemId = %a.ItemId
                  ResolvedId = %a.ResolvedId
                  GivenUrl = GivenUrl givenUrl
                  ResolvedUrl = ResolvedUrl resolvedUrl
                  AmpUrl = ampUrl |> Option.map AmpUrl
                  GivenTitle = %a.GivenTitle
                  ResolvedTitle = %a.ResolvedTitle
                  Favorite = favorite
                  Status = status
                  Excerpt = %a.Excerpt
                  IsArticle = isArticle
                  WordCount = %a.WordCount
                  ListenDurationEstimate = %a.ListenDurationEstimate
                  TimeToRead = %a.TimeToRead
                  TimeAdded = % DateTimeOffset.FromUnixTimeSeconds(int64 a.TimeAdded)
                  TimeUpdated = % DateTimeOffset.FromUnixTimeSeconds(int64 a.TimeUpdated) }

            return y
        }
