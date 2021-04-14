module Pocindle.Playground.Sql

open System.Data.Common

open FSharp.Data.LiteralProviders
open Dapper
open Npgsql

type qsql = TextFile<"q.sql">

let asd (connection: #DbConnection) a =

    let u =
        connection.QuerySingle<string>(qsql.Text, a)

    u

let zxc () =
    use con =
        new NpgsqlConnection("Host=localhost;Database=postgres;Username=postgres;Password=qwerty123")

    let e = asd con {| Id = 7 |}

    printfn $"%s{qsql.Text}\n%A{e}"
