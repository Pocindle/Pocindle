module Pocindle.Pocket.Domain

open Pocindle.Pocket.SimpleTypes

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
      AmpUrl: AmpUrl option
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
