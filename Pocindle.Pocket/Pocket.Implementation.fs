module Pocindle.Pocket.Pocket_Implementation

open Pocindle.Pocket.Pocket
open Pocindle.Pocket.Pocket.Builder

let imp1 a b c =
    Pocindle.Pocket.Auth.Implementation.obtainRequestToken a b c

let imp2 a b = unimplemented ""
let imp3 a b c = unimplemented ""

let rec interpret =
    function
    | Pure x -> x
    | Free (OAuthRequest (a, b, c, next)) -> (a, b, c) |||> imp1 |> next |> interpret
    | Free (OAuthAuthorize (a, b, next)) -> (a, b) ||> imp2 |> next |> interpret
    | Free (Retrieve (a, b, c, next)) -> (a, b, c) |||> imp3 |> next |> interpret
   
open Pocindle.Pocket.Pocket.CE
open Pocindle.Pocket.Common.SimpleTypes
open Pocindle.Pocket.Auth.SimpleTypes

let sample () =
    let y =
        freePocket {
            let! a =
                Pocket.oAuthAuthorize
                    (ConsumerKey.create "dsa" |> Result.get)
                    (RequestToken.create "" |> Result.get)
                    None

            return a
        }

    let r = y |> interpret

    r
