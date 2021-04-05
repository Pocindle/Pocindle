module Pocindle.Pocket.SimpleTypes

open System
open FSharp.UMX

open Pocindle.Domain.SimpleTypes

module Retrieve =
    [<Measure>]
    type private itemId

    [<Measure>]
    type private resolvedId

    [<Measure>]
    type private givenTitle

    [<Measure>]
    type private resolvedTitle

    [<Measure>]
    type private excerpt

    [<Measure>]
    type private wordCount

    [<Measure>]
    type private listenDurationEstimate

    [<Measure>]
    type private timeToRead

    [<Measure>]
    type private timeAdded

    [<Measure>]
    type private timeUpdated

    type ItemId = string<itemId>

    type ResolvedId = string<resolvedId>

    type GivenUrl = GivenUrl of Uri

    type ResolvedUrl = ResolvedUrl of Uri

    type GivenTitle = string<givenTitle>

    type ResolvedTitle = string<resolvedTitle>

    type Excerpt = string<excerpt>

    type WordCount = int<wordCount>

    type ListenDurationEstimate = int<listenDurationEstimate>

    type AmpUrl = AmpUrl of Uri option

    type TimeToRead = TimeToRead of int option

    type TimeAdded = DateTimeOffset<timeAdded>

    type TimeUpdated = DateTimeOffset<timeUpdated>

module Auth =
    [<Measure>]
    type private state

    type State = string<state> option

    type ConsumerKey = private ConsumerKey of string

    module ConsumerKey =
        let value (ConsumerKey key) = key

        let create str =
            ConstrainedType.createFixedString "ConsumerKey" ConsumerKey 30 str

    type RequestToken = private RequestToken of string

    module RequestToken =
        let value (RequestToken key) = key

        let create str =
            ConstrainedType.createFixedString "RequestToken" RequestToken 30 str

    type RedirectUri =
        | RedirectUri of Uri
        | RedirectString of string

    module RedirectUri =
        let valueStr =
            function
            | RedirectUri uri -> uri.ToString()
            | RedirectString str -> str

        let withRequestToken (RequestToken requestToken) uri =
            match uri with
            | RedirectUri u -> RedirectUri ^ Uri(u, requestToken)
            | RedirectString s -> RedirectString $"{s}{requestToken}"

    type AccessToken = private AccessToken of string

    module AccessToken =
        let value (AccessToken key) = key

        let create str =
            ConstrainedType.createFixedString "AccessToken" AccessToken 30 str

    type Username = private Username of string


    module Username =
        let value (Username key) = key

        let create str =
            ConstrainedType.createString "Username" Username 100 str
