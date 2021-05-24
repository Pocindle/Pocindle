module Pocindle.Database.Users

open System
open System.Net.Mail
open System.Threading.Tasks

open FSharp.Data.LiteralProviders
open FsToolkit.ErrorHandling
open FsToolkit.ErrorHandling.Operator.Result
open FSharp.UMX

open Pocindle.Domain.SimpleTypes
open Pocindle.Database.Database
open Pocindle.Domain

type private UserData =
    { userid: int64
      pocketusername: string
      ``to``: string
      from: string
      smtpserver: string
      password: string }

let getUserFromPocketUsername connectionString (username: PocketUsername) =
    taskResult {
        let! ret =
            querySingle<UserData, _>
                connectionString
                TextFile.Users.``UserFromPocketUsername.sql``.Text
                (Some {| PocketUsername = PocketUsername.value username |})
            |> TaskResult.mapError DbException


        let! ret1 =
            match ret with
            | Some r ->
                result {
                    let! pocketUsername =
                        PocketUsername.create r.pocketusername
                        |> Result.mapError ValidationError

                    let deliveryConfig =
                        match r.smtpserver |> Option.ofObj with
                        | Some _ ->
                            { SmtpServer = SmtpServer.toDomain r.smtpserver |> Result.get
                              From = MailAddress(r.from)
                              To = MailAddress(r.``to``)
                              Password = r.password }
                            |> Some
                        | None -> None

                    return!
                        Ok
                            { User.UserId = %r.userid
                              PocketUsername = pocketUsername
                              DeliveryConfig = deliveryConfig }
                }
            | None -> Error Empty

        return ret1
    }

let getAccessTokenFromPocketUsername connectionString (username: PocketUsername) =
    taskResult {
        let! ret =
            (querySingle
                connectionString
                TextFile.Users.``AccessTokenFromPocketUsername.sql``.Text
                (Some {| PocketUsername = PocketUsername.value username |}))
            |> TaskResult.mapError DbException

        let! ret1 =
            match ret with
            | Some r ->
                AccessToken.create r
                |> Result.mapError ValidationError
            | None -> Error Empty

        return ret1
    }

let setAccessTokenByPocketUsername connectionString (username: PocketUsername) (accessToken: AccessToken) =
    taskResult {
        let! ret =
            execute
                connectionString
                TextFile.Users.``UpdateAccessTokenByPocketUsername.sql``.Text
                {| AccessToken = AccessToken.value accessToken
                   PocketUsername = PocketUsername.value username |}
            |> TaskResult.mapError DbException

        return! matchExecuteResult ret
    }

let createUser connectionString (username: PocketUsername) (accessToken: AccessToken) =
    taskResult {
        let! ret =
            execute
                connectionString
                TextFile.Users.``CreateUser.sql``.Text
                {| AccessToken = AccessToken.value accessToken
                   PocketUsername = PocketUsername.value username |}
            |> TaskResult.mapError DbException

        return! matchExecuteResult ret
    }

let getUserIdByPocketUsername connectionString (username: PocketUsername) =
    taskResult {
        let! ret =
            (querySingle<int64, _>
                connectionString
                TextFile.Users.``getUserIdByPocketUsername.sql``.Text
                (Some {| PocketUsername = PocketUsername.value username |}))
            |> TaskResult.mapError DbException

        let! (ret1: UserId) =
            match ret with
            | Some r -> Ok(%r)
            | None -> Error Empty

        return ret1
    }

let setKindleMailAddressByUserId connectionString (userId: UserId) (kindleMail: KindleEmailAddress) =
    taskResult {
        let! ret =
            execute
                connectionString
                TextFile.Users.``SetKindleEmailAddressByUserId.sql``.Text
                {| KindleEmailAddress = KindleEmailAddress.fromDomain kindleMail
                   UserId = %userId |}
            |> TaskResult.mapError DbException

        return! matchExecuteResult ret
    }
