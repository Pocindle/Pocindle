module Pocindle.Pocket.Domain

open Pocindle.Pocket.SimpleTypes
open Pocindle.Pocket.SimpleTypes.Retrieve

type Favorite =
    | Favorite
    | NotFavorite

type Status =
    | Normal
    | Archived
    | ShouldBeDeleted

type PocketItem =
    { ItemId: ItemId
      ResolvedId: ResolvedId
      GivenUrl: GivenUrl
      ResolvedUrl: ResolvedUrl
      AmpUrl: AmpUrl
      GivenTitle: GivenTitle
      ResolvedTitle: ResolvedTitle
      Favorite: Favorite
      Status: Status
      Excerpt: Excerpt
      IsArticle: bool
      WordCount: WordCount
      ListenDurationEstimate: ListenDurationEstimate
      TimeToRead: TimeToRead
      TimeAdded: TimeAdded
      TimeUpdated: TimeUpdated }
