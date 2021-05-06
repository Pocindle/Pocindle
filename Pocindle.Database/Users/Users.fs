module Pocindle.Database.Users

open System
open System.Threading.Tasks

open FSharp.Data.LiteralProviders
open FsToolkit.ErrorHandling
open FsToolkit.ErrorHandling.Operator.Result
open FSharp.UMX

open Pocindle.Domain.SimpleTypes
open Pocindle.Database.Database
open Pocindle.Domain

let getUserFromPocketUsername connectionString (username: PocketUsername) =
    taskResult {
        let! ret =
            querySingle<{| UserId: int64
                           PocketUsername: string
                           KindleEmailAddress: string |}, _>
                connectionString
                TextFile.Users.``UserFromPocketUsername.sql``.Text
                (Some {| PocketUsername = PocketUsername.value username |})
            |> TaskResult.mapError DbException


        let! ret1 =
            match ret with
            | Some r ->
                result {
                    let! pocketUsername =
                        PocketUsername.create r.PocketUsername
                        |> Result.mapError ValidationError

                    return!
                        Ok
                            { User.UserId = %r.UserId
                              PocketUsername = pocketUsername
                              KindleEmailAddress =
                                  r.KindleEmailAddress
                                  |> Option.ofObj
                                  |> Option.map (System.Net.Mail.MailAddress >> KindleEmailAddress) }
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

        return!
            (match ret with
             | 0 -> Error Empty
             | 1 -> Ok()
             | r -> Error ^ TooMuchAffected r)
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

        return!
            (match ret with
             | 0 -> Error Empty
             | 1 -> Ok()
             | r -> Error ^ TooMuchAffected r)
    }
