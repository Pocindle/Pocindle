module Pocindle.Common.Serialization

open System
open System.Text.Json

type SerializeError =
    | ArgumentException of ArgumentException
    | NotSupportedException of NotSupportedException

let serialize a =
    try
        Ok ^ JsonSerializer.Serialize(a)
    with
    | :? ArgumentException as ex -> Error ^ ArgumentException ex
    | :? NotSupportedException as ex -> Error ^ NotSupportedException ex


type DeserializeError =
    | JsonException of JsonException
    | NotSupportedException of NotSupportedException

let deserialize<'T> (a: string) =
    try
        Ok ^ JsonSerializer.Deserialize<'T>(a)
    with
    | :? JsonException as ex -> Error ^ JsonException ex
    | :? NotSupportedException as ex -> Error ^ NotSupportedException ex

type JsonError =
    | SerializationError
    | DeserializationError
