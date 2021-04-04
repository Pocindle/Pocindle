module Server

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Saturn
open Config

let endpointPipe =
    pipeline {
        plug head
        plug requestId
    }

let app =
    application {
        pipe_through endpointPipe

        error_handler (fun ex _ -> pipeline { render_html (InternalError.layout ex) })
        use_router Router.appRouter
        url "http://localhost:61666/"
        memory_cache
        use_static "static"
        use_static "pocindle-client/build"
        use_gzip
        use_config (fun _ -> { connectionString = "DataSource=database.sqlite" }) //TODO: Set development time configuration

        use_developer_exceptions

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
    run app
    0
