module Pocindle.Saturn.Core

open System.IO
open Microsoft.AspNetCore.Http

open FSharp.Control.Tasks
open Giraffe.Core
open Giraffe

let customFile contentType (filePath: string) =
    fun (_: HttpFunc) (ctx: HttpContext) ->
        task {
            let filePath =
                match Path.IsPathRooted filePath with
                | true -> filePath
                | false ->
                    let env = ctx.GetHostingEnvironment()
                    Path.Combine(env.ContentRootPath, filePath)

            ctx.SetContentType(contentType + "; charset=utf-8")

            let! html = readFileAsStringAsync filePath
            return! ctx.WriteStringAsync html
        }

let jsonFile (filePath: string) : HttpHandler = customFile ApplicationJson filePath
