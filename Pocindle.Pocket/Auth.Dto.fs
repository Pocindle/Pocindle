module Pocindle.Pocket.Auth.Dto

open System.ComponentModel.DataAnnotations

open Pocindle.Pocket.Auth.SimpleTypes

type RequestDto =
    { [<Required>]
      RequestToken: string
      [<Required>]
      RedirectUrl: string }

module RequestDto =
    let fromDomain (requestToken: RequestToken) (redirectUrl: PocketRedirectUri) =
        { RequestToken = RequestToken.value requestToken
          RedirectUrl = PocketRedirectUri.valueStr redirectUrl }
