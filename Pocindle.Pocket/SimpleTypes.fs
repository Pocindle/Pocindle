module Pocindle.Pocket.SimpleTypes

open System
open FSharp.UMX

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

type AmpUrl = AmpUrl of Uri

type TimeToRead = int<timeToRead>

type TimeAdded = DateTimeOffset<timeAdded>

type TimeUpdated = DateTimeOffset<timeUpdated>
