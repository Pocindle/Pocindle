module Pocindle.Saturn.Auth

open System
open Saturn
open System.Security.Claims
open System.IdentityModel.Tokens.Jwt
open Microsoft.IdentityModel.Tokens
open Giraffe
open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http

open Pocindle.Pocket.Domain.Auth
open Pocindle.Pocket.SimpleTypes.Auth

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

let generateTokenViaPocket (requestToken: RequestToken) (username: Username) =
    let claims =
        [| Claim(JwtRegisteredClaimNames.Sub, Username.value username)
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
            let consumer_key =
                ConsumerKey.create ""
                |> Result.get

            let redirect_uri =
                RedirectString "https://pocindle.xyz/authorizationFinished/"

            let! y = Pocindle.Pocket.Auth.obtainRequestToken consumer_key redirect_uri None

            match y with
            | Ok (t, _) ->
                let tr =
                    Pocindle.Pocket.Dto.Auth.RequestDto.fromDomain t (RedirectUri.withRequestToken t redirect_uri)

                return! json tr next ctx
            | Error ex -> return! (setStatusCode 500 >=> json ex) next ctx
        }

let authorize =
    fun (requestToken: string) (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let consumer_key =
                ConsumerKey.create ""
                |> Result.get

            let requestToken =
                RequestToken.create requestToken |> Result.get

            let! y = Pocindle.Pocket.Auth.authorize consumer_key requestToken

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
        not_found_handler (setStatusCode 404 >=> text "Not Found")

        post "/request" request
        postf "/authorize/%s" authorize

        post "/token" handlePostToken
        get "/" (text "public route")
        forward "/secured" securedRouter
    }
