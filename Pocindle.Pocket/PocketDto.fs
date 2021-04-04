module Pocindle.Pocket.PocketDto

open System
open System.Collections.Generic

open FsToolkit.ErrorHandling
open FSharp.UMX

open Pocindle.Pocket.Domain
open Pocindle.Pocket.SimpleTypes

type PocketItemPocketDto =
    { item_id: string
      resolved_id: string
      given_url: string
      given_title: string
      favorite: string
      status: string
      time_added: string
      time_updated: string
      time_read: string
      time_favorited: string
      sort_id: int
      resolved_title: string
      resolved_url: string
      excerpt: string
      is_article: string
      is_index: string
      has_video: string
      has_image: string
      word_count: string
      lang: string
      time_to_read: Nullable<int>
      amp_url: string
      top_image_url: string
      listen_duration_estimate: int }

type PocketRetrieveRootPocketDto =
    { status: int
      complete: int
      list: IDictionary<string, PocketItemPocketDto>
      error: Object
      since: int64 }

module PocketItemPocketDto =
    let toDomain (a: PocketItemPocketDto) =
        result {
            try
                let! givenUrl =
                    match Uri.TryCreate(a.given_url, UriKind.Absolute) with
                    | true, b -> Ok b
                    | _ -> Error "Invalid given_url"

                let! resolvedUrl =
                    match Uri.TryCreate(a.resolved_url, UriKind.Absolute) with
                    | true, b -> Ok b
                    | _ -> Error "Invalid resolved_url"

                let! ampUrl =
                    match Uri.TryCreate(a.amp_url, UriKind.Absolute) with
                    | true, b -> Ok(Some b)
                    | _ when String.IsNullOrEmpty(a.amp_url) -> Ok None
                    | _ -> Error "Invalid amp_url"

                let! favorite =
                    match a.favorite with
                    | "0" -> Ok NotFavorite
                    | "1" -> Ok Favorite
                    | _ -> Error "Invalid favorite "

                let! status =
                    match a.status with
                    | "0" -> Ok Normal
                    | "1" -> Ok Archived
                    | "2" -> Ok ShouldBeDeleted
                    | _ -> Error "Invalid status"

                let! isArticle =
                    match a.is_article with
                    | "0" -> Ok false
                    | "1" -> Ok true
                    | _ -> Error "Invalid is_article"

                let! wordCount =
                    match Int32.TryParse a.word_count with
                    | true, b -> Ok b
                    | _ -> Error "Invalid word_count"

                let timeToRead =
                    a.time_to_read |> Option.ofNullable |> TimeToRead

                let y =
                    { ItemId = %a.item_id
                      ResolvedId = %a.resolved_id
                      GivenUrl = GivenUrl givenUrl
                      ResolvedUrl = ResolvedUrl resolvedUrl
                      AmpUrl = AmpUrl ampUrl
                      GivenTitle = %a.given_title
                      ResolvedTitle = %a.resolved_title
                      Favorite = favorite
                      Status = status
                      Excerpt = %a.excerpt
                      IsArticle = isArticle
                      WordCount = %wordCount
                      ListenDurationEstimate = %a.listen_duration_estimate
                      TimeToRead = timeToRead
                      TimeAdded = % DateTimeOffset.FromUnixTimeSeconds(int64 a.time_added)
                      TimeUpdated = % DateTimeOffset.FromUnixTimeSeconds(int64 a.time_updated) }

                return y
            with ex -> return! Error ^ string ex
        }
