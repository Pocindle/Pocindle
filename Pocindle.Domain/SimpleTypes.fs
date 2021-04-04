module Pocindle.Domain.SimpleTypes

open System
open FsToolkit.ErrorHandling

type UserId = UserId of uint64

type String50 = private String50 of string
type String1000 = private String1000 of string

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

module String50 =
    let value (String50 str) = str

    let create fieldName str =
        ConstrainedType.createString fieldName String50 50 str

    let createOption fieldName str =
        ConstrainedType.createStringOption fieldName String50 50 str

module String1000 =
    let value (String1000 str) = str

    let create fieldName str =
        ConstrainedType.createString fieldName String1000 1000 str

    let createOption fieldName str =
        ConstrainedType.createStringOption fieldName String1000 1000 str
