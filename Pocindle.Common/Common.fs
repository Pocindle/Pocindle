[<AutoOpen>]
module Pocindle

open System

type Undefined = exn

let inline unimplemented a =
    match a with
    | "" -> raise <| NotImplementedException ()
    | _ -> raise <| NotImplementedException a
 