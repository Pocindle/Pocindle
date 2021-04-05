[<AutoOpen>]
module Pocindle

open System

type Undefined = exn

let inline unimplemented a =
    match a with
    | "" -> raise <| NotImplementedException()
    | _ -> raise <| NotImplementedException a

module Result =
    let get =
        function
        | Ok t -> t
        | _ -> invalidArg "result" "The Result value was Error"
