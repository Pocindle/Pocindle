[<AutoOpen>]
module Pocindle.Common

open System

let inline unimplemented a =
    match a with
    | "" -> raise <| NotImplementedException ()
    | _ -> raise <| NotImplementedException a
 