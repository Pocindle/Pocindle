module Pocindle.Pocket.Auth.Dto

open System
open System.ComponentModel.DataAnnotations

open FSharp.UMX
open FsToolkit.ErrorHandling

open Pocindle.Pocket.Auth.SimpleTypes

type RequestDto =
    { [<Required>]
      RequestToken: string
      [<Required>]
      RedirectUrl: string }

module RequestDto =
    let fromDomain (requestToken: RequestToken) (redirectUrl: RedirectUri) =
        { RequestToken = RequestToken.value requestToken
          RedirectUrl = RedirectUri.valueStr redirectUrl }
