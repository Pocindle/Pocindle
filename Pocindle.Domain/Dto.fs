module Pocindle.Domain.Dto

open System.ComponentModel.DataAnnotations
open System.Net.Mail

open FSharp.UMX
open FsToolkit.ErrorHandling
open Pocindle.Domain.SimpleTypes

type DeliveryDto =
    { [<Required>]
      UserId: uint64
      [<Required>]
      To: string }

module DeliveryDto =
    let toDomain a =
        result {
            try
                let userId = a.UserId
                let email = KindleEmailAddress(MailAddress(a.To))

                return
                    { Delivery.UserId = %userId
                      Data = unimplemented ""
                      To = email }
            with ex -> return! Error ^ string ex
        }

    let fromDomain (a: Delivery) =
        let userId = a.UserId
        let (KindleEmailAddress email) = a.To

        { DeliveryDto.UserId = %userId
          To = email.ToString() }
