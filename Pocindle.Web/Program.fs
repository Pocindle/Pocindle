module Pocindle.Web.App

open System
open System.IO
open System.Text
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Configuration.Json
open Microsoft.Extensions.Configuration.UserSecrets
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.IdentityModel.Tokens

open FSharp.UMX
open Giraffe
open Giraffe.EndpointRouting
open Npgsql

open Pocindle.Domain.SimpleTypes
open Pocindle.Web.Router

let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")

    clearResponse
    >=> setStatusCode 500
    >=> text ex.Message

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
        .UseAuthentication()
        .UseRouting()
        .UseAuthentication()
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
    let ic = sp.GetService<IConfiguration>()

    let config = Config.buildConfig ic |> Result.get

    services.AddSingleton<Config> config |> ignore

    services
        .AddAuthorization()
        .AddAuthentication(fun cfg ->
            cfg.DefaultAuthenticateScheme <- JwtBearerDefaults.AuthenticationScheme
            cfg.DefaultChallengeScheme <- JwtBearerDefaults.AuthenticationScheme
            cfg.DefaultScheme <- JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(fun options ->

            //options.SaveToken <- true
            //options.IncludeErrorDetails <- true
            //options.Authority <- "https://accounts.google.com"
            //options.Audience <- %config.JwtIssuer
            options.TokenValidationParameters <-
                TokenValidationParameters(
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = %config.JwtIssuer,
                    ValidAudience = %config.JwtIssuer,
                    IssuerSigningKey = SymmetricSecurityKey(Encoding.UTF8.GetBytes(%config.JwtSecret))
                ))
    |> ignore

let configureLogging (builder: ILoggingBuilder) =
    builder.AddConsole().AddDebug() |> ignore

[<EntryPoint>]
let main args =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot = Path.Combine(contentRoot, "WebRoot")

    printfn $"%A{webApp}"

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
