module Pocindle.Pocket.Dto

open System
open System.ComponentModel.DataAnnotations

open FSharp.UMX
open FsToolkit.ErrorHandling

open Pocindle.Pocket.Domain

module Retrieve =
    open Pocindle.Pocket.SimpleTypes.Retrieve
    open Pocindle.Pocket.Domain.Retrieve

    type StatusDto =
        | Normal = 0
        | Archived = 1
        | ShouldBeDeleted = 2

    type PocketItemDto =
        { [<Required>]
          ItemId: string
          [<Required>]
          ResolvedId: string
          [<Required>]
          GivenUrl: string
          [<Required>]
          ResolvedUrl: string
          AmpUrl: string
          [<Required>]
          GivenTitle: string
          [<Required>]
          ResolvedTitle: string
          [<Required>]
          Favorite: bool
          [<Required>]
          Status: StatusDto
          [<Required>]
          Excerpt: string
          [<Required>]
          IsArticle: bool
          [<Required>]
          WordCount: int
          [<Required>]
          ListenDurationEstimate: int
          TimeToRead: Nullable<int>
          [<Required>]
          TimeAdded: DateTimeOffset
          [<Required>]
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
                  | Normal -> StatusDto.Normal
                  | Archived -> StatusDto.Archived
                  | ShouldBeDeleted -> StatusDto.ShouldBeDeleted
              Excerpt = %a.Excerpt
              IsArticle = a.IsArticle
              WordCount = %a.WordCount
              ListenDurationEstimate = %a.ListenDurationEstimate
              TimeToRead = timeToRead |> Option.toNullable
              TimeAdded = %a.TimeAdded
              TimeUpdated = %a.TimeUpdated }

module Auth =
    open Pocindle.Pocket.SimpleTypes.Auth
    open Pocindle.Pocket.Domain.Auth

    type RequestDto =
        { [<Required>]
          RequestToken: string
          [<Required>]
          RedirectUrl: string }

    module RequestDto =
        let fromDomain (requestToken: RequestToken) (redirectUrl: RedirectUri) =
            { RequestToken = RequestToken.value requestToken
              RedirectUrl = RedirectUri.valueStr redirectUrl }
