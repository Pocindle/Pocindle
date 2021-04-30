module Pocindle.Web.Core

open System
open System.IO
open Microsoft.AspNetCore.Http

open FSharp.Control.Tasks
open Giraffe.Core
open Giraffe
open Giraffe.EndpointRouting

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

let acceptJson : HttpHandler = mustAccept [ ApplicationJson ]

let routefd (path: PrintfFormat<_, _, _, _, 'T>) (routeHandler: 'T -> HttpHandler) : Endpoint =
    routef path routeHandler |> addMetadata typeof<'T>

let routefdd
    (path: PrintfFormat<_, _, _, _, 'T>)
    (routeHandler: 'T -> HttpHandler, routeResponses: (int * Type) list)
    : Endpoint =
    let r =
        routef path routeHandler |> addMetadata typeof<'T>

    (r, routeResponses)
    ||> List.fold (fun st t -> st |> addMetadata t)

let routed path (routeHandler: HttpHandler, routeResponses: (int * Type) list) : Endpoint =
    let r = route path routeHandler

    (r, routeResponses)
    ||> List.fold (fun st t -> st |> addMetadata t)

let queryStringValuesToOption (qs: Microsoft.Extensions.Primitives.StringValues) =
    if qs.Count = 0 then
        None
    else
        Some qs.[0]
