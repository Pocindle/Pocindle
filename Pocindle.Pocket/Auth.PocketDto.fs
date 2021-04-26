module Pocindle.Pocket.Auth.PocketDto

open FSharp.UMX

open Pocindle.Pocket.Auth.SimpleTypes
open Pocindle.Domain.SimpleTypes

type ObtainRequestTokenRequestDto =
    { consumer_key: string
      redirect_uri: string
      state: string }

module ObtainRequestTokenRequestDto =
    let fromDomain (consumer_key: ConsumerKey) (redirect_uri: RedirectUri) (state: State) =
        { consumer_key = ConsumerKey.value consumer_key
          redirect_uri = RedirectUri.valueStr redirect_uri
          state = state |> Option.map (~%) |> Option.toObj }

type ObtainRequestTokenResponseDto = { code: string; state: string }

type AuthorizeRequestDto = { consumer_key: string; code: string }

module AuthorizeRequestDto =
    let fromDomain (consumer_key: ConsumerKey) (code: RequestToken) =
        { AuthorizeRequestDto.consumer_key = ConsumerKey.value consumer_key
          code = RequestToken.value code }

type AuthorizeResponseDto =
    { access_token: string
      username: string }
