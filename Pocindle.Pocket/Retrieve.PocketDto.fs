module Pocindle.Pocket.Retrieve.PocketDto

open System
open System.Collections.Generic

open FsToolkit.ErrorHandling
open FSharp.UMX
open FsToolkit.ErrorHandling.Operator.Result
open Pocindle.Domain.SimpleTypes
open Pocindle.Pocket.Retrieve.PublicTypes
open Pocindle.Pocket.Retrieve.SimpleTypes

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

module PocketItemPocketDto =
    let toDomain (a: PocketItemPocketDto) =
        result {
            try
                let! givenUrl =
                    match Uri.TryCreate(a.given_url, UriKind.Absolute) with
                    | true, b -> Ok b
                    | _ -> Error "Invalid given_url"

                let resolvedUrl =
                    match Uri.TryCreate(a.resolved_url, UriKind.Absolute) with
                    | true, b -> b
                    | _ -> givenUrl

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

type RetrieveResponsePocketDto =
    { status: int
      complete: int
      list: IDictionary<string, PocketItemPocketDto>
      error: Object
      since: int64 }

module RetrieveResponsePocketDto =
    let toDomain a =
        let items =
            a.list.Values
            |> List.ofSeq
            |> List.map PocketItemPocketDto.toDomain

        let errors =
            items
            |> List.filter Result.isError
            |> List.map Result.getError

        let response =
            { Since = a.since |> DateTimeOffset.FromUnixTimeSeconds
              Items =
                  items
                  |> List.filter Result.isOk
                  |> List.map Result.get }

        response, errors

type RetrieveOptionalParametersQuery = list<struct (string * string)>

module RetrieveOptionalParametersQuery =
    let private stateFromDomain =
        function
        | Unread -> "unread"
        | Archive -> "archive"
        | All -> "All"

    let private favouriteFromDomain =
        function
        | Favorite -> "1"
        | NotFavorite -> "0"

    let private tagFromDomain =
        function
        | TagName n -> n
        | Untagged -> "_untagged_"

    let private contentTypeFromDomain =
        function
        | Article -> "article"
        | Video -> "video"
        | Image -> "image"

    let private sortFromDomain =
        function
        | Newest -> "newest"
        | Oldest -> "oldest"
        | Title -> "title"
        | Site -> "site"

    let private detailTypeFromDomain =
        function
        | Simple -> "simple"
        | Complete -> "complete"

    let private dateTimeOffsetToTimestamp (d: DateTimeOffset) = d.ToUnixTimeSeconds()

    let toQuery (a: RetrieveOptionalParameters) =
        let a =
            [ struct ("state", a.State |> Option.map stateFromDomain)
              struct ("favorite", a.Favorite |> Option.map favouriteFromDomain)
              struct ("tag", a.Tag |> Option.map tagFromDomain)
              struct ("contentType", a.ContentType |> Option.map contentTypeFromDomain)
              struct ("sort", a.Sort |> Option.map sortFromDomain)
              struct ("detailType", a.DetailType |> Option.map detailTypeFromDomain)
              struct ("search", a.Search)
              struct ("domain", a.Domain)
              struct ("since",
                      a.Since
                      |> Option.map (dateTimeOffsetToTimestamp >> string))
              struct ("count", a.Count |> Option.map string)
              struct ("offset", a.Offset |> Option.map string) ]

        a
        |> List.filter (fun (struct (_, b)) -> Option.isSome b)
        |> List.map (fun (struct (a, b)) -> struct (a, Option.get b))

module ConsumerKey =
    let toQuery ck =
        struct ("consumer_key", ConsumerKey.value ck)

module AccessToken =
    let toQuery ck =
        struct ("access_token", AccessToken.value ck)
