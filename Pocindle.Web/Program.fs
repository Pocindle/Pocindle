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
open Microsoft.IdentityModel.Tokens
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
    let ic = sp.GetService<IConfiguration>()

    Config.buildConfig ic
    |> services.AddSingleton<Config>
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
                    ValidIssuer = ic.["JwtIssuer"],
                    ValidAudience = ic.["JwtIssuer"],
                    IssuerSigningKey = SymmetricSecurityKey(Encoding.UTF8.GetBytes(ic.["JwtSecret"]))
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
