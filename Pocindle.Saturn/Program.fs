module Server

open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Configuration.Json
open Microsoft.Extensions.Configuration

open FsToolkit.ErrorHandling
open Saturn
open Saturn.Endpoint

open Pocindle.Pocket.Common.SimpleTypes
open Pocindle.Pocket.Retrieve.PocketDto
open Pocindle.Saturn


let endpointPipe =
    pipeline {
        plug head
        plug requestId
    }

let app =
    application {
        pipe_through endpointPipe

        //error_handler (fun ex _ -> pipeline { render_html (InternalError.layout ex) })
        use_endpoint_router Router.appRouter
        url "http://localhost:61666/"
        memory_cache
        use_static "static"
        use_static "pocindle-client/build"
        use_gzip

        use_config
            (fun ic ->
                { ConsumerKey =
                      ic.["Pocket:ConsumerKey"]
                      |> ConsumerKey.create
                      |> Result.get
                  ConnectionString = ic.GetConnectionString("DefaultConnection") })

        use_developer_exceptions

        use_jwt_authentication Auth.secret Auth.issuer

        app_config
            (fun app ->
                let env =
                    Application.Environment.getWebHostEnvironment app

                app)

        service_config (fun services -> services)

        webhost_config (fun w -> w)
    }

[<EntryPoint>]
let main _ =
    printfn $"Working directory - %s{System.IO.Directory.GetCurrentDirectory()}"
    printfn "%A" Router.appRouter
    run app
    0
