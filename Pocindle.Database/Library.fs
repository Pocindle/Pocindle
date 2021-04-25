module Pocindle.Database.Users

open System.Net.Mime.MediaTypeNames
open System.Threading.Tasks

open FSharp.Data.LiteralProviders
open FsToolkit.ErrorHandling
open Npgsql

open Pocindle.Domain.SimpleTypes
open Pocindle.Database.Database
open Pocindle.Domain

type UserFromPocketUsernameQuery = TextFile<"Users/UserFromPocketUsername.sql">


let getUserFromPocketUsername connectionString (username: PocketUsername) : QuerySingleResult<User> =
    let connection = new NpgsqlConnection(connectionString)

    taskResult {
        let! ret =
            querySingle
                connection
                UserFromPocketUsernameQuery.Text
                (Some {| PocketUsername = PocketUsername.value username |})

        return ret
    }
    
//type AccessTokenFromPocketUsernameQuery = TextFile<"Users/AccessTokenFromPocketUsername.sql">

let getAccessTokenFromPocketUsername connectionString (username: PocketUsername) : QuerySingleResult<string> =
    let connection = new NpgsqlConnection(connectionString)

    taskResult {
        let! ret =
            querySingle
                connection
                TextFile.Users.``AccessTokenFromPocketUsername.sql``.Text
                (Some {| PocketUsername = PocketUsername.value username |})

        return ret
    }
