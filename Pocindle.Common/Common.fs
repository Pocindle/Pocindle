[<AutoOpen>]
module Global

open System
open System.Threading.Tasks

open FSharp.Control.Tasks

type Undefined = exn

let inline (^) a b = a b

let inline unimplemented a =
    match a with
    | "" -> raise ^ NotImplementedException()
    | _ -> raise ^ NotImplementedException a

let inline raise500 a = a |> string |> Exception |> raise

module Option =
    let toResult error opt =
        match opt with
        | Some a -> Ok a
        | None -> Error error

module Result =
    let inline get result =
        match result with
        | Ok t -> t
        | _ -> invalidArg "result" "The Result value was Error"

    let inline getError result =
        match result with
        | Error t -> t
        | _ -> invalidArg "result" "The Result value was Ok"

    let inline tryCall f a =
        try
            f a |> Ok
        with ex -> Error ex

    let liftOk res =
        match res with
        | Ok ok ->
            match ok with
            | Ok a -> Ok a
            | Error b -> Error b
        | Error b -> Error b

module AsyncResult =
    let inline get asyncResult =
        async {
            match! asyncResult with
            | Ok t -> return t
            | _ -> return invalidArg "asyncResult" "The AsyncResult value was Error"
        }

    let inline getError asyncResult =
        async {
            match! asyncResult with
            | Error t -> return t
            | _ -> return invalidArg "asyncResult" "The AsyncResult value was Ok"
        }

    let inline tryCall f a =
        async {
            try
                let! t = f a
                return Ok t
            with ex -> return Error ex
        }

module TaskResult =
    let inline get (taskResult: Task<Result<_, _>>) =
        task {
            match! taskResult with
            | Ok t -> return t
            | _ -> return invalidArg "taskResult" "The TaskResult value was Error"
        }

    let inline getError (taskResult: Task<Result<_, _>>) =
        task {
            match! taskResult with
            | Error t -> return t
            | _ -> return invalidArg "taskResult" "The TaskResult value was Ok"
        }

    let inline tryCall (f: _ -> Task<_>) a =
        task {
            try
                let! t = f a
                return Ok t
            with ex -> return Error ex
        }

[<Literal>]
let XAccept = "X-Accept"

[<Literal>]
let ApplicationJson = System.Net.Mime.MediaTypeNames.Application.Json
