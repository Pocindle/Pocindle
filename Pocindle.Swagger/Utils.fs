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

let templateToList (template: string) =
    let spl1 =
        template.Split([| '/' |], StringSplitOptions.RemoveEmptyEntries)

    let param =
        spl1
        |> Array.filter (fun s -> s.[0] = '{' && s.[s.Length - 1] = '}')

    let withoutBrackets =
        param
        |> Array.map (fun s -> s.Substring(1, s.Length - 2))

    withoutBrackets |> Array.toList

let extractType meta =
    let t, other =
        meta
        |> List.partition
            (fun (t: obj) ->
                match t with
                | :? Type -> true
                | _ -> false)

    t
    |> List.tryHead
    |> Option.map (fun o -> o :?> Type),
    other

let extractResponseTypes meta =
    meta
    |> List.filter
        (fun (t: obj) ->
            match t with
            | :? (int * Type) -> true
            | _ -> false)
    |> List.map (fun o -> o :?> int * Type)
