module Pocindle.Domain.SimpleTypes

open System

open FSharp.UMX
open FsToolkit.ErrorHandling

[<Measure>]
type private userId

type UserId = int64<userId>

[<Measure>]
type private deliveryId

type DeliveryId = int64<deliveryId>

module ConstrainedType =
    let createString fieldName ctor maxLen str =
        result {
            if String.IsNullOrEmpty(str) then
                return! Error $"%s{fieldName} must not be null or empty"
            elif str.Length > maxLen then
                return! Error $"%s{fieldName} must not be more than %i{maxLen} chars"
            else
                return ctor str
        }

    let createFixedString fieldName ctor len str =
        result {
            if String.IsNullOrEmpty(str) then
                return! Error $"%s{fieldName} must not be null or empty"
            elif str.Length <> len then
                return! Error $"%s{fieldName} must be %i{len} chars"
            else
                return ctor str
        }

    let createStringOption fieldName ctor maxLen str =
        if String.IsNullOrEmpty(str) then
            Ok None
        elif str.Length > maxLen then
            Error $"%s{fieldName} must not be more than %i{maxLen} chars"
        else
            Ok(ctor str |> Some)

    let createInt fieldName ctor minVal maxVal i =
        if i < minVal then
            Error $"%s{fieldName}: Must not be less than %i{minVal}"
        elif i > maxVal then
            Error $"%s{fieldName}: Must not be greater than %i{maxVal}"
        else
            Ok(ctor i)

    let createDecimal fieldName ctor minVal maxVal i =
        if i < minVal then
            Error $"%s{fieldName}: Must not be less than %M{minVal}"
        elif i > maxVal then
            Error $"%s{fieldName}: Must not be greater than %M{maxVal}"
        else
            Ok(ctor i)

    let createLike fieldName ctor pattern str =
        if String.IsNullOrEmpty(str) then
            Error $"%s{fieldName}: Must not be null or empty"
        elif System.Text.RegularExpressions.Regex.IsMatch(str, pattern) then
            Ok(ctor str)
        else
            Error $"%s{fieldName}: '%s{str}' must match the pattern '%s{pattern}'"


type PocketUsername = private PocketUsername of string

module PocketUsername =
    let value (PocketUsername key) = key

    let create str =
        ConstrainedType.createString "PocketUsername" PocketUsername 100 str

type ConsumerKey = private ConsumerKey of string

module ConsumerKey =
    let value (ConsumerKey key) = key

    let create str =
        ConstrainedType.createFixedString "ConsumerKey" ConsumerKey 30 str

    let toQuery (ConsumerKey key) = struct ("consumer_key", key)

type AccessToken = private AccessToken of string

module AccessToken =
    let value (AccessToken key) = key

    let create str =
        ConstrainedType.createFixedString "AccessToken" AccessToken 30 str

    let toQuery (AccessToken key) = struct ("access_token", key)


type SpaUrl = private SpaUrl of Uri

module SpaUrl =
    let value (SpaUrl spaUrl) = spaUrl

    let fromString str = str |> Uri |> SpaUrl

type SmtpServer =
    | SmtpServerWithPort of string * int
    | SmtpServer of string

module SmtpServer =
    let toDomain (s: string) =
        try
            match s.Split(":") |> List.ofArray with
            | [ host; port ] -> Ok ^ SmtpServerWithPort(host, int port)
            | [ host ] -> Ok ^ SmtpServer host
            | _ -> invalidArg s "s"
        with ex -> Error ^ ex

    let fromDomain s =
        match s with
        | SmtpServerWithPort (h, p) -> h, p
        | SmtpServer h -> h, 587
