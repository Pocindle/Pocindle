module Pocindle.Common.UriQuery

open System.Web

let fromValueTuple (a: struct (string * string) seq) =
    let query = HttpUtility.ParseQueryString("")

    for struct (field, value) in a do
        query.[field] <- value

    string query
