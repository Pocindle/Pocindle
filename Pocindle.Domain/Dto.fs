module Pocindle.Domain.Dto

open System.ComponentModel.DataAnnotations
open System.Net.Mail

open FSharp.UMX
open FsToolkit.ErrorHandling
open Pocindle.Domain.SimpleTypes


type UserDto =
    { [<Required>]
      UserId: int64
      [<Required>]
      PocketUsername: string
      KindleEmailAddress: string }

module UserDto =
    let fromDomain (user: User) =
        { UserDto.UserId = %user.UserId
          PocketUsername = user.PocketUsername |> PocketUsername.value
          KindleEmailAddress =
              match user.KindleEmailAddress with
              | HasKindleEmailAddress mail -> string mail
              | NoneKindleEmailAddress -> null }

type DeliveryDto =
    { [<Required>]
      UserId: int64
      [<Required>]
      To: string }

//module DeliveryDto =
//    let toDomain a =
//        result {
//            try
//                let userId = a.UserId
//                let email = KindleEmailAddress(MailAddress(a.To))
//
//                return
//                    { Delivery.UserId = %userId
//                      Data = unimplemented ""
//                      To = email }
//            with ex -> return! Error ^ string ex
//        }
//
//    let fromDomain (a: Delivery) =
//        let userId = a.UserId
//        let (KindleEmailAddress email) = a.To
//
//        { DeliveryDto.UserId = %userId
//          To = email.ToString() }
