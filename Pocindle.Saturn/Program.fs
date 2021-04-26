module Server

open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Configuration.UserSecrets
open Microsoft.Extensions.Configuration.Json
open Microsoft.Extensions.Configuration

open FsToolkit.ErrorHandling
open Npgsql
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

let app  =
    application {
        pipe_through endpointPipe

        //error_handler (fun ex _ -> pipeline { render_html (InternalError.layout ex) })
        use_endpoint_router Router.appRouter
        //url $"http://localhost:%d{port}/"
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
                  ConnectionString =
                      let builder =
                          NpgsqlConnectionStringBuilder(ic.GetConnectionString("DefaultConnection"))

                      builder.Password <- ic.["DbPassword"]
                      builder.ConnectionString
                  BaseUrl = ic.["BaseUrl"] |> Uri })

        use_developer_exceptions

        use_jwt_authentication Auth.secret Auth.issuer


        app_config
            (fun app ->
                let env =
                    Application.Environment.getWebHostEnvironment app

                app.UseSwaggerUI(fun c -> c.SwaggerEndpoint("/openapi.json", "qwerty"))
                |> ignore


                app)

        service_config (fun services -> services)

        host_config
            (fun host ->
                host.ConfigureAppConfiguration(
                    System.Action<_, _>
                        (fun (a: HostBuilderContext) (b: IConfigurationBuilder) ->
                            if a.HostingEnvironment.IsDevelopment() then
                                b.AddUserSecrets<Config>() |> ignore)
                ))

        webhost_config (fun w -> w)

        logging (fun l -> ())
    }

[<EntryPoint>]
let main args =
    printfn $"Working directory - %s{Directory.GetCurrentDirectory()}"
    printfn "%A" Router.appRouter
    //run ^ app (args |> Array.head |> int)
    run app
    0
