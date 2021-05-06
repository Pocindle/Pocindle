module Pocindle.Database.Database

open System.Threading.Tasks

open Dapper
open Npgsql
open FSharp.Control.Tasks
open FSharp.UMX

open Pocindle.Database

type ExecuteResult = Task<Result<int, exn>>
type QueryResult<'T> = Task<Result<'T seq, exn>>
type QuerySingleResult<'T> = Task<Result<'T option, exn>>

let execute (connectionString: ConnectionString) (sql: string) (parameters: _) : ExecuteResult =
    let connection = new NpgsqlConnection(%connectionString)

    task {
        try
            let! res = connection.ExecuteAsync(sql, parameters)
            return Ok res
        with ex -> return Error ex
    }

let query<'T, 'U> (connectionString: ConnectionString) (sql: string) (parameters: 'U option) : QueryResult<'T> =
    let connection = new NpgsqlConnection(%connectionString)

    task {
        try
            let! res =
                match parameters with
                | Some p -> connection.QueryAsync<'T>(sql, p)
                | None -> connection.QueryAsync<'T>(sql)

            return Ok res
        with ex -> return Error ex
    }

let querySingle<'T, 'U>   (connectionString: ConnectionString) (sql: string) (parameters: 'U option) : QuerySingleResult<'T> =
    let connection = new NpgsqlConnection(%connectionString)

    task {
        try
            let! res =
                match parameters with
                | Some p -> connection.QuerySingleOrDefaultAsync<'T>(sql, p)
                | None -> connection.QuerySingleOrDefaultAsync<'T>(sql)

            return
                if isNull (box res) then
                    Ok None
                else
                    Ok(Some res)

        with ex -> return Error ex
    }
