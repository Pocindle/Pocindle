module Pocindle.Pocket.Auth.PublicTypes

open System.Threading.Tasks

open Pocindle.Common.Serialization
open Pocindle.Domain.SimpleTypes
open Pocindle.Pocket.Auth.SimpleTypes

type AuthError =
    | Exception of exn
    | FetchException of exn
    | ValidationError
    | ParseError of string
    | SerializationError of SerializationError
    | DeserializationError of DeserializationError

type ObtainRequestToken = ConsumerKey -> RedirectUri -> State -> Task<Result<RequestToken * State, AuthError>>

type Authorize = ConsumerKey -> RequestToken -> Task<Result<AccessToken * PocketUsername, AuthError>>
