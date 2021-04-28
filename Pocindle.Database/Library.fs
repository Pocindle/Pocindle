﻿module Pocindle.Database.Users

open System
open System.Threading.Tasks

open FSharp.Data.LiteralProviders
open FsToolkit.ErrorHandling
open FsToolkit.ErrorHandling.Operator.Result
open Npgsql

open Pocindle.Domain.SimpleTypes
open Pocindle.Database.Database
open Pocindle.Domain

let getUserFromPocketUsername connectionString (username: PocketUsername) =
    let connection = new NpgsqlConnection(connectionString)

    taskResult {
        let! ret =
            querySingle
                connection
                TextFile.Users.``UserFromPocketUsername.sql``.Text
                (Some {| PocketUsername = PocketUsername.value username |})
            |> TaskResult.mapError DbException

        return
            (match ret with
             | Some (r: User) -> Ok r
             | None -> Error Empty)
    }

let getAccessTokenFromPocketUsername connectionString (username: PocketUsername) =
    let connection = new NpgsqlConnection(connectionString)

    taskResult {
        let! ret =
            (querySingle
                connection
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
    let connection = new NpgsqlConnection(connectionString)

    taskResult {
        let! ret =
            execute
                connection
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
    let connection = new NpgsqlConnection(connectionString)

    taskResult {
        let! ret =
            execute
                connection
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
