module Pocindle.Pocket.Retrieve.SimpleTypes

open System
open System.Net.Http
open System.Net.Http
open System.Text
open System.Text.Json

open FSharp.UMX
open FSharp.Control.Tasks
open FsToolkit.ErrorHandling

open Pocindle.Domain.SimpleTypes

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
