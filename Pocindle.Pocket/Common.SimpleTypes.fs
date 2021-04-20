module Pocindle.Pocket.Common.SimpleTypes

open System
open FSharp.UMX

open Pocindle.Domain.SimpleTypes

type ConsumerKey = private ConsumerKey of string

module ConsumerKey =
    let value (ConsumerKey key) = key

    let create str =
        ConstrainedType.createFixedString "ConsumerKey" ConsumerKey 30 str

    let toQuery (ConsumerKey key) = struct ("consumer_key", key)

type AccessToken = private AccessToken of string

module AccessToken =
    let value (AccessToken key) = key

    let create str =
        ConstrainedType.createFixedString "AccessToken" AccessToken 30 str

    let toQuery (AccessToken key) = struct ("access_token", key)
