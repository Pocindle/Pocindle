module Pocindle.Swagger.Utils

open System

let tupleTypeToList (t: Type) =
    if t.IsGenericType then
        t.GenericTypeArguments |> List.ofArray
    else
        t |> List.singleton

let tupleTypeOptionToList t =
    match t |> Option.map tupleTypeToList with
    | Some a -> a
    | None -> []
