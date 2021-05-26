module Pocindle.Common.Serialization

open System
open System.Text.Json

type SerializationError =
    | ArgumentException of ArgumentException
    | NotSupportedException of NotSupportedException

let serialize a =
    try
        Ok ^ JsonSerializer.Serialize(a)
    with
    | :? ArgumentException as ex -> Error ^ ArgumentException ex
    | :? NotSupportedException as ex -> Error ^ NotSupportedException ex


type DeserializationError =
    | JsonException of JsonException
    | NotSupportedException of NotSupportedException

let deserialize<'T> (a: string) =
    try
        Ok ^ JsonSerializer.Deserialize<'T>(a)
    with
    | :? JsonException as ex -> Error ^ JsonException ex
    | :? NotSupportedException as ex -> Error ^ NotSupportedException ex

let emptyOptions = JsonSerializerOptions()
