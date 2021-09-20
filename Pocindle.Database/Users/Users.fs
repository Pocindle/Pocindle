module Pocindle.Database.Users

open FSharp.Data.LiteralProviders
open FsToolkit.ErrorHandling
open FsToolkit.ErrorHandling.Operator.Result
open FSharp.UMX

open Pocindle.Database.Db
open Pocindle.Domain.SimpleTypes
open Pocindle.Database.Database
open Pocindle.Domain
open SqlHydra.Query

let getUserFromPocketUsername (qctx: QueryContext) (username: PocketUsername) =
    taskResult {
        let username = PocketUsername.value username

        let! ret =
            select {
                for i in Tables.users do
                    where (i.pocketusername = username)
                    select i
            }
            |> qctx.ReadAsync HydraReader.Read

        let! ret1 =
            match ret |> Seq.tryHead with
            | Some r ->
                result {
                    let! pocketUsername =
                        PocketUsername.create r.pocketusername
                        |> Result.mapError ValidationError

                    return!
                        Ok
                            { User.UserId = %r.userid
                              PocketUsername = pocketUsername
                              KindleEmailAddress =
                                  r.kindleemailaddress
                                  |> KindleEmailAddress.ofOption }
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
