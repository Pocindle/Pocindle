module Pocindle.Saturn.Auth

open System
open Saturn
open System.Security.Claims
open System.IdentityModel.Tokens.Jwt
open Microsoft.IdentityModel.Tokens
open Giraffe
open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http

let secret = "spadR2dre#u-ruBrE@TepA&*Uf@U"
let issuer = "pocindle.xyz"


[<CLIMutable>]
type LoginViewModel = { Email: string; Password: string }

[<CLIMutable>]
type TokenResult = { Token: string }

let generateToken email =
    let claims =
        [| Claim(JwtRegisteredClaimNames.Sub, email)
           Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) |]

    claims
    |> Auth.generateJWT (secret, SecurityAlgorithms.HmacSha256) issuer (DateTime.UtcNow.AddHours(1.0))

let handleGetSecured =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        let email =
            ctx.User.FindFirst ClaimTypes.NameIdentifier

        text
            ("User "
             + email.Value
             + " is authorized to access this resource.")
            next
            ctx

let handlePostToken =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! model = ctx.BindFormAsync<LoginViewModel>()

            // authenticate user

            let tokenResult = generateToken model.Email

            return! json tokenResult next ctx
        }

let securedRouter =
    router {
        pipe_through (Auth.requireAuthentication JWT)
        get "/" handleGetSecured
    }

let topRouter =
    router {
        not_found_handler (setStatusCode 404 >=> text "Not Found")

        post "/token" handlePostToken
        get "/" (text "public route")
        forward "/secured" securedRouter
    }
