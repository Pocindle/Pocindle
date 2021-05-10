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

type PocindleRedirectPrefix = private PocindleRedirectPrefix of string

module PocindleRedirectPrefix =
    let empty = "" |> PocindleRedirectPrefix

    let fromSpaUrl spaUrl =
        SpaUrl.value spaUrl
        |> string
        |> PocindleRedirectPrefix

    let valueStr (PocindleRedirectPrefix str) = str

type PocindleRedirectUri = PocindleRedirectUri of Uri

module PocindleRedirectUri =
    let fromPocindleRedirectString (RequestToken requestToken) spaUrl =
        let str = SpaUrl.value spaUrl |> string
        (if str.EndsWith "/" then
             $"%s{str}authorizationFinished/%s{requestToken}"
         else
             $"%s{str}/authorizationFinished/%s{requestToken}")
        |> Uri
        |> PocindleRedirectUri

    let valueStr (PocindleRedirectUri uri) = string uri

type PocketRedirectUri = PocketRedirectUri of Uri

module PocketRedirectUri =
    let valueStr (PocketRedirectUri uri) = string uri

    let withRequestTokenAndPocindleRedirectUri (RequestToken requestToken) (uri: PocindleRedirectUri) =
        let q =
            [ struct ("request_token", requestToken)
              struct ("redirect_uri", uri |> PocindleRedirectUri.valueStr) ]

        let ret =
            UriBuilder("https://getpocket.com/auth/authorize")

        ret.Query <- Pocindle.Common.UriQuery.fromValueTuple q
        PocketRedirectUri ret.Uri
