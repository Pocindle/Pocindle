module Pocindle.Pocket.Dto

open System

open FSharp.UMX

open FsToolkit.ErrorHandling
open Pocindle.Pocket.Domain
open Pocindle.Pocket.SimpleTypes

type PocketItemDto =
    { ItemId: string
      ResolvedId: string
      GivenUrl: string
      ResolvedUrl: string
      AmpUrl: string
      GivenTitle: string
      ResolvedTitle: string
      Favorite: bool
      Status: string
      Excerpt: string
      IsArticle: bool
      WordCount: int
      ListenDurationEstimate: int
      TimeToRead: Nullable<int>
      TimeAdded: DateTimeOffset
      TimeUpdated: DateTimeOffset }

module PocketItemDto =
    let fromDomain (a: PocketItem) =
        let (GivenUrl givenUrl) = a.GivenUrl
        let (ResolvedUrl resolvedUrl) = a.ResolvedUrl
        let (TimeToRead timeToRead) = a.TimeToRead
        let (AmpUrl ampUrl) = a.AmpUrl

        { ItemId = %a.ItemId
          ResolvedId = %a.ResolvedId
          GivenUrl = givenUrl.ToString()
          ResolvedUrl = resolvedUrl.ToString()
          AmpUrl =
              ampUrl
              |> Option.map (fun r -> r.ToString())
              |> Option.defaultValue null
          GivenTitle = %a.GivenTitle
          ResolvedTitle = %a.ResolvedTitle
          Favorite =
              match a.Favorite with
              | Favorite -> true
              | NotFavorite -> false
          Status =
              match a.Status with
              | Normal -> "Normal"
              | Archived -> "Archived"
              | ShouldBeDeleted -> "ShouldBeDeleted"
          Excerpt = %a.Excerpt
          IsArticle = a.IsArticle
          WordCount = %a.WordCount
          ListenDurationEstimate = %a.ListenDurationEstimate
          TimeToRead = timeToRead |> Option.toNullable
          TimeAdded = %a.TimeAdded
          TimeUpdated = %a.TimeUpdated }
