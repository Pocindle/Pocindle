module Pocindle.Web.Convert


open System
open System.Security.Claims
open System.Threading.Tasks
open FsToolkit.ErrorHandling.Operator.TaskResult
open Microsoft.AspNetCore.Http

open FSharp.Control.Tasks
open FSharp.UMX
open Giraffe
open FsToolkit.ErrorHandling

open Pocindle.Domain
open Pocindle.Domain.Dto
open Pocindle.Domain.SimpleTypes
open Pocindle.Pocket.Retrieve
open Pocindle.Pocket.Retrieve.PublicTypes
open Pocindle.Database.Users
open Pocindle.Database
open Pocindle.Convert.Api
open Pocindle.Convert.Domain

let private articleToFiles articleUrl =
    taskResult {
        let! epub = convertWebToEpub Pandoc (Article(Uri(articleUrl)))
        let! mobi = convertEpubToMobi Calible epub
        return epub, mobi
    }

//let convert =
//    (fun (articleUrl: string) next (ctx: HttpContext) -> //return raise500 error
//        task {
//            let config = ctx.GetService<Config>()
//
//            let! a =
//                taskResult {
//                    let! pocketUsername =
//                        ctx.User.FindFirst ClaimTypes.NameIdentifier
//                        |> fun claim -> claim.Value |> PocketUsername.create
//
//                    let! epub, mobi =
//                        articleToFiles articleUrl
//                        |> TaskResult.mapError string
//
//                    
//                    
//                    return em
//                }
//
//
//
//
//            return! json () next ctx
//
//        }),
//    []
