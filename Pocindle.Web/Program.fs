module Pocindle.Web.App

open System
open System.IO
open System.Text
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Authentication.JwtBearer
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
open Microsoft.IdentityModel.Protocols.OpenIdConnect
open Microsoft.IdentityModel.Tokens
open Npgsql
open Pocindle.Domain.SimpleTypes
open Pocindle.Web.Config

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
        let c =
            ctx.GetService<Config.Config>().ConnectionString

        match ctx.GetHostingEnvironment().IsDevelopment() with
        | true -> redirectTo false "http://localhost:3000" next ctx
        | false -> htmlFile "index.html" next ctx

let webApp =
    [ GET [ route "/" serveSpa
            route "/2" (indexHandler "world")
            routef "/hello/%s" indexHandler ] ]

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


let configureApp (app: IApplicationBuilder) =
    let env =
        app.ApplicationServices.GetService<IWebHostEnvironment>()

    let config =
        app.ApplicationServices.GetService<Config>()

    let configureCors (builder: CorsPolicyBuilder) =
        builder
            .WithOrigins(string config.BaseUrl)
            .AllowAnyMethod()
            .AllowAnyHeader()
        |> ignore

    app
        .UseRouting()
        .UseGiraffe(webApp)
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseSwaggerUI(fun c -> c.SwaggerEndpoint("/openapi.json", "qwerty"))
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

    let sp = services.BuildServiceProvider()
    let env = sp.GetService<IHostEnvironment>()
    let c = sp.GetService<IConfiguration>()

    let config =
        { Config.Config.ConsumerKey =
              c.["Pocket:ConsumerKey"]
              |> ConsumerKey.create
              |> Result.get
          Config.Config.ConnectionString =
              let builder =
                  NpgsqlConnectionStringBuilder(c.GetConnectionString("DefaultConnection"))

              builder.Password <- c.["DbPassword"]
              builder.ConnectionString
          Config.Config.BaseUrl = c.["BaseUrl"] |> Uri }

    services.AddSingleton<Config.Config>(config)
    |> ignore

    services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(fun options ->
            options.TokenValidationParameters <-
                TokenValidationParameters(
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = c.["JwtIssuer"],
                    ValidAudience = c.["JwtIssuer"],
                    IssuerSigningKey = SymmetricSecurityKey(Encoding.UTF8.GetBytes(c.["JwtSecret"]))
                ))
    |> ignore

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
