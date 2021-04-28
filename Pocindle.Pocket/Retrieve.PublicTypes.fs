module Pocindle.Pocket.Retrieve.PublicTypes

open System
open System.Threading.Tasks

open Pocindle.Common.Serialization
open Pocindle.Domain.SimpleTypes
open Pocindle.Pocket.Retrieve.SimpleTypes

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

type State =
    | Unread
    | Archive
    | All

type Tag =
    | TagName of string
    | Untagged

type ContentType =
    | Article
    | Video
    | Image

type Sort =
    | Newest
    | Oldest
    | Title
    | Site

type DetailType =
    | Simple
    | Complete

type RetrieveOptionalParameters =
    { State: State option
      Favorite: Favorite option
      Tag: Tag option
      ContentType: ContentType option
      Sort: Sort option
      DetailType: DetailType option
      Search: string option
      Domain: string option
      Since: DateTimeOffset option
      Count: int option
      Offset: int option }

module RetrieveOptionalParameters =
    let empty =
        { State = None
          Favorite = None
          Tag = None
          ContentType = None
          Sort = None
          DetailType = None
          Search = None
          Domain = None
          Since = None
          Count = None
          Offset = None }

type RetrieveResponse =
    { Items: PocketItem list
      Since: DateTimeOffset }

type RetrieveError =
    | FetchException of exn
    | SerializationError of SerializationError
    | DeserializationError of DeserializationError
    | ValidationError of string list

type Retrieve = ConsumerKey -> AccessToken -> RetrieveOptionalParameters -> Task<Result<RetrieveResponse, RetrieveError>>
