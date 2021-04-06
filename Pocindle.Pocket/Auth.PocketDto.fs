module Pocindle.Pocket.Auth.PocketDto

type ObtainRequestTokenRequestDto =
    { consumer_key: string
      redirect_uri: string
      state: string }

type ObtainRequestTokenResponseDto = { code: string; state: string }

type AuthorizeRequestDto = { consumer_key: string; code: string }

type AuthorizeResponseDto =
    { access_token: string
      username: string }
