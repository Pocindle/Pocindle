module Pocindle.Saturn.Auth

open System
open System.Security.Claims
open System.IdentityModel.Tokens.Jwt
open Microsoft.AspNetCore.Http
open Microsoft.IdentityModel.Tokens

open FSharp.Control.Tasks
open Giraffe
open Saturn
open Saturn.Endpoint

open Pocindle.Domain.SimpleTypes
open Pocindle.Pocket.Auth.PublicTypes
open Pocindle.Pocket.Auth.SimpleTypes
open Pocindle.Pocket.Common.SimpleTypes
open Pocindle.Pocket.Common
open Pocindle.Pocket.Auth.Dto

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

let generateTokenViaPocket (requestToken: RequestToken) (username: PocketUsername) =
    let claims =
        [| Claim(JwtRegisteredClaimNames.Sub, PocketUsername.value username)
           Claim(JwtRegisteredClaimNames.Iss, issuer)
           Claim(JwtRegisteredClaimNames.UniqueName, RequestToken.value requestToken)
           Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) |]

    claims
    |> Auth.generateJWT (secret, SecurityAlgorithms.HmacSha256) issuer (DateTime.UtcNow.AddDays(14.))

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

let request =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let consumer_key = (Controller.getConfig ctx).ConsumerKey 

            let redirect_uri =
                RedirectString "https://pocindle.xyz/authorizationFinished/"

            let! y = Pocindle.Pocket.Auth.Api.obtainRequestToken consumer_key redirect_uri None

            match y with
            | Ok (t, _) ->
                let tr =
                    RequestDto.fromDomain t (RedirectUri.withRequestToken t redirect_uri)

                return! json tr next ctx
            | Error ex -> return! (setStatusCode 500 >=> json ex) next ctx
        }

let authorize =
    fun (requestToken: string) (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let consumer_key = (Controller.getConfig ctx).ConsumerKey 

            let requestToken =
                RequestToken.create requestToken |> Result.get

            let! y = Pocindle.Pocket.Auth.Api.authorize consumer_key requestToken

            match y with
            | Ok (t, a) ->
                let jwtToken = generateTokenViaPocket requestToken a
                return! json jwtToken next ctx
            | Error ex -> return! (setStatusCode 500 >=> json ex) next ctx
        }

let securedRouter =
    router {
        pipe_through (Auth.requireAuthentication JWT)
        get "/" handleGetSecured
    }

let topRouter =
    router {
        //not_found_handler (setStatusCode 404 >=> text "Not Found")

        post "/request" request
        postf "/authorize/%s" authorize

        post "/token" handlePostToken
        get "/" (text "public route")
        post "/" (text "public route")
        forward "/secured" securedRouter
    }
