module Pocindle.Web.Auth

open System
open System.Text
open System.Security.Claims
open System.IdentityModel.Tokens.Jwt
open Microsoft.AspNetCore.Http
open Microsoft.IdentityModel.Tokens
open Microsoft.AspNetCore.Authentication.JwtBearer

open FSharp.Control.Tasks
open FSharp.UMX
open Giraffe

open Pocindle.Domain.SimpleTypes
open Pocindle.Pocket.Auth.Dto
open Pocindle.Pocket.Auth.SimpleTypes
open Pocindle.Database

let authorizeJwt : HttpHandler =
    requiresAuthentication (challenge JwtBearerDefaults.AuthenticationScheme)


let generateTokenViaPocket (requestToken: RequestToken) (username: PocketUsername) (ctx: HttpContext) =
    let config = ctx.GetService<Config>()

    let claims =
        [| Claim(JwtRegisteredClaimNames.Sub, PocketUsername.value username)
           Claim(JwtRegisteredClaimNames.Iss, %config.JwtIssuer)
           Claim(JwtRegisteredClaimNames.UniqueName, RequestToken.value requestToken)
           Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid() |> string) |]

    let expires = Nullable(DateTime.UtcNow.AddHours(1.0))
    let notBefore = Nullable(DateTime.UtcNow)

    let securityKey =
        SymmetricSecurityKey(Encoding.UTF8.GetBytes(%config.JwtSecret))

    let signingCredentials =
        SigningCredentials(key = securityKey, algorithm = SecurityAlgorithms.HmacSha256)

    let token =
        JwtSecurityToken(
            issuer = %config.JwtIssuer,
            audience = %config.JwtIssuer,
            claims = claims,
            expires = expires,
            notBefore = notBefore,
            signingCredentials = signingCredentials
        )

    JwtSecurityTokenHandler().WriteToken(token)

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

let request =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let config = ctx.GetService<Config>()
            let consumer_key = config.ConsumerKey

            let redirect_str =
                PocindleRedirectString "https://pocindle.xyz/authorizationFinished/"

            let! y = Pocindle.Pocket.Auth.Api.obtainRequestToken consumer_key redirect_str None

            match y with
            | Ok (t, _) ->
                let redirect_uri =
                    PocindleRedirectUri.fromPocindleRedirectString t redirect_str

                let tr =
                    RequestDto.fromDomain t (PocketRedirectUri.withRequestTokenAndPocindleRedirectUri t redirect_uri)

                return! json tr next ctx
            | Error ex -> return raise500 ex
        }

let authorize =
    fun (requestToken: string) (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let config = ctx.GetService<Config>()
            let consumer_key = config.ConsumerKey

            let requestToken =
                RequestToken.create requestToken |> Result.get

            let! y = Pocindle.Pocket.Auth.Api.authorize consumer_key requestToken

            match y with
            | Ok (t, a) ->
                let jwtToken =
                    generateTokenViaPocket requestToken a ctx

                let! c = Users.setAccessTokenByPocketUsername %config.ConnectionString a t

                match c with
                | Ok _ -> return! json jwtToken next ctx
                | Error DbError.Empty ->
                    let! b = Users.createUser %config.ConnectionString a t

                    match b with
                    | Ok _ -> return! json jwtToken next ctx
                    | err -> return raise500 err
                | Error err -> return raise500 err
            | Error ex -> return raise500 ex
        }
