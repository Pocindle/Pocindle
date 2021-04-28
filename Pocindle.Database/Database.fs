module Pocindle.Database.Database

open System.Threading.Tasks

open Dapper
open System.Data.Common
open FSharp.Control.Tasks

type DbError =
    | DbException of exn
    | ValidationError of string
    | Empty
    | TooMuchAffected of int
    
type ExecuteResult = Task<Result<int, exn>>
type QueryResult<'T> = Task<Result<'T seq, exn>>
type QuerySingleResult<'T> = Task<Result<'T option, exn>>

let execute (connection: #DbConnection) (sql: string) (parameters: _) : ExecuteResult =
    task {
        try
            let! res = connection.ExecuteAsync(sql, parameters)
            return Ok res
        with ex -> return Error ex
    }

let query (connection: #DbConnection) (sql: string) (parameters: _) : QueryResult<_> =
    task {
        try
            let! res =
                match parameters with
                | Some p -> connection.QueryAsync<'T>(sql, p)
                | None -> connection.QueryAsync<'T>(sql)

            return Ok res
        with ex -> return Error ex
    }

let querySingle (connection: #DbConnection) (sql: string) (parameters: _) : QuerySingleResult<_> =
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
