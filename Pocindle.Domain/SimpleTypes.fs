module Pocindle.Domain.SimpleTypes

open System

open FSharp.UMX
open FsToolkit.ErrorHandling

[<Measure>]
type private userId

type UserId = uint64<userId>


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
