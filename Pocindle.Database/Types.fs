namespace Pocindle.Database

open FSharp.UMX

[<Measure>]
type private connectionString

type ConnectionString = string<connectionString>

type DbError =
    | DbException of exn
    | ValidationError of string
    | Empty
    | TooMuchAffected of int
