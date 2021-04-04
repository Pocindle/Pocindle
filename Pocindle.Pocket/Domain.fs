module Pocindle.Pocket.Domain

type ItemId = Undefined
type ResolvedId = Undefined
type GivenUrl = Undefined
type ResolvedUrl = Undefined
type GivenTitle = Undefined
type ResolvedTitle = Undefined
type Excerpt = Undefined
type WordCount = Undefined
type ListenDurationEstimate = Undefined
type AmpUrl = Undefined
type TimeToRead = Undefined

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
      TimeToRead: TimeToRead }
