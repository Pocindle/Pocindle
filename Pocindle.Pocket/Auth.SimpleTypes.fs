module Pocindle.Pocket.Auth.SimpleTypes

open System

open FSharp.UMX

open Pocindle.Domain.SimpleTypes

[<Measure>]
type private state

type State = string<state> option

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
