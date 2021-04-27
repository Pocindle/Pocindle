module Pocindle.Web.Config

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.ResponseCompression
open Giraffe
open Microsoft.AspNetCore
open System
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open System.IO
open Microsoft.AspNetCore.Rewrite
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Authentication.Cookies
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.StaticFiles
open Microsoft.Extensions.Configuration
open Microsoft.AspNetCore.Authentication
open FSharp.Control.Tasks
open System.Net.Http
open System.Net.Http.Headers
open System.Threading.Tasks
open Microsoft.Extensions.Hosting
open Giraffe.EndpointRouting
open System
open Pocindle.Domain.SimpleTypes

type Config =
    { ConnectionString: string
      ConsumerKey: ConsumerKey
      BaseUrl: Uri }

let useConfig configBuilder endpoints =
    let mutable (x: Config option) = None

    let handler (nxt: HttpFunc) (ctx: HttpContext) : HttpFuncResult =
        let v =
            match x with
            | None ->
                let ic = ctx.GetService<IConfiguration>()
                let v = configBuilder ic
                x <- Some v
                v
            | Some v -> v

        ctx.Items.["Configuration"] <- v
        nxt ctx

    endpoints
    |> List.map (fun e -> applyBefore handler e)

let getConfig (ctx: HttpContext) =
    unbox<Config> ctx.Items.["Configuration"]
