module Pocindle.Web.App

open System
open System.IO
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
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
open Microsoft.AspNetCore.Hosting

open Giraffe
open Giraffe.EndpointRouting
open Npgsql
open Pocindle.Domain.SimpleTypes

// ---------------------------------
// Models
// ---------------------------------

type Message = { Text: string }

// ---------------------------------
// Views
// ---------------------------------

module Views =
    open Giraffe.ViewEngine

    let layout (content: XmlNode list) =
        html [] [
            head [] [
                title [] [ encodedText "Pocindle.Web" ]
                link [ _rel "stylesheet"
                       _type "text/css"
                       _href "/main.css" ]
            ]
            body [] content
        ]

    let partial () = h1 [] [ encodedText "Pocindle.Web" ]

    let index (model: Message) =
        [ partial ()
          p [] [ encodedText model.Text ] ]
        |> layout

// ---------------------------------
// Web app
// ---------------------------------

let indexHandler (name: string) =
    let greetings = sprintf "Hello %s, from Giraffe!" name
    let model = { Text = greetings }
    let view = Views.index model
    htmlView view

let serveSpa : HttpHandler =
    fun next ctx ->
        let c = Config.getConfig ctx
        match ctx.GetHostingEnvironment().IsDevelopment() with
        | true -> redirectTo false "http://localhost:3000" next ctx
        | false -> htmlFile "index.html" next ctx

let webApp =
    [ GET [ route "/" serveSpa
            route "/2" (indexHandler "world")
            routef "/hello/%s" indexHandler ] ]

let webAppC =
    Config.useConfig
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
        webApp

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")

    clearResponse
    >=> setStatusCode 500
    >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder: CorsPolicyBuilder) =
    builder
        .WithOrigins("http://localhost:5000", "https://localhost:5001")
        .AllowAnyMethod()
        .AllowAnyHeader()
    |> ignore

let configureApp (app: IApplicationBuilder) =
    let env =
        app.ApplicationServices.GetService<IWebHostEnvironment>()

    app
        .UseRouting()
        .UseGiraffe(webAppC)
        .UseCors(configureCors)
        .UseStaticFiles()
    |> ignore

    match env.IsDevelopment() with
    | true -> app.UseDeveloperExceptionPage()
    | false ->
        app
            .UseGiraffeErrorHandler(errorHandler)
            .UseHttpsRedirection()
    |> ignore

let configureServices (services: IServiceCollection) =
    services.AddCors() |> ignore
    services.AddRouting() |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder: ILoggingBuilder) =
    builder.AddConsole().AddDebug() |> ignore

[<EntryPoint>]
let main args =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot = Path.Combine(contentRoot, "WebRoot")

    Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .UseKestrel()
                .UseContentRoot(contentRoot)
                .UseWebRoot(webRoot)
                .Configure(Action<IApplicationBuilder> configureApp)
                .ConfigureServices(configureServices)
                .ConfigureLogging(configureLogging)
            |> ignore)
        .Build()
        .Run()

    0
