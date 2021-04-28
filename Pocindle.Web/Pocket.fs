module Pocindle.Web.Pocket

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

open FsToolkit.ErrorHandling
open Pocindle.Domain.SimpleTypes
open Pocindle.Pocket.Auth.Dto
open Pocindle.Pocket.Auth.SimpleTypes
open Pocindle.Pocket.Retrieve
open Pocindle.Pocket.Retrieve.PublicTypes
open Pocindle.Database.Users

let retrieveAll =
    fun next (ctx: HttpContext) ->
        task {
            let config = ctx.GetService<Config>()

            let! retrieve =
                taskResult {
                    let! pocketUsername =
                        ctx.User.FindFirst ClaimTypes.NameIdentifier
                        |> fun claim -> claim.Value
                        |> PocketUsername.create

                    let! accessToken =
                        getAccessTokenFromPocketUsername %config.ConnectionString pocketUsername
                        |> TaskResult.mapError string

                    let consumerKey = config.ConsumerKey

                    return Api.retrieve consumerKey accessToken
                }

            let! p =
                match retrieve with
                | Ok retrieve -> retrieve RetrieveOptionalParameters.empty
                | Error validationError -> unimplemented validationError

            match p with
            | Ok response ->
                let dto =
                    Dto.PocketRetrieveDto.fromDomain response

                return! (json dto next ctx)
            | Error a -> return! (json a next ctx)
        }
