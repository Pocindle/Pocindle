module Server

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer
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
        use_gzip
        use_config (fun _ -> { connectionString = "DataSource=database.sqlite" }) //TODO: Set development time configuration

        use_developer_exceptions

        app_config
            (fun app ->
                let env =
                    Application.Environment.getWebHostEnvironment app

                //app.UseStaticFiles()
                //app.UseSpaStaticFiles()

                app.UseSpa
                    (fun spa ->
                        spa.Options.SourcePath <- "../pocindle-client"

                        if env.IsDevelopment() then
                            spa.UseReactDevelopmentServer("start")

                        ())

                app)

        service_config (fun services ->
            services.AddSpaStaticFiles(fun configuration -> configuration.RootPath <- "pocindle-client/build" )         
            services)
        
        webhost_config
            (fun w ->

                w)
    }

[<EntryPoint>]
let main _ =
    printfn "Working directory - %s" (System.IO.Directory.GetCurrentDirectory())
    run app
    0 // return an integer exit code
