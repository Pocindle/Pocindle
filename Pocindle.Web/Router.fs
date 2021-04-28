module Pocindle.Web.Router

open Microsoft.AspNetCore.Hosting

open Giraffe
open Giraffe.EndpointRouting

open Pocindle.Web.Auth
open Pocindle.Web.Pocket
open Pocindle.Web.Core

let serveSpa : HttpHandler =
    fun next ctx ->
        let c = ctx.GetService<Config>()

        match ctx.GetHostingEnvironment().IsDevelopment() with
        | true -> redirectTo false (string c.BaseUrl) next ctx
        | false -> htmlFile "index.html" next ctx

let webApp =
    [ GET [ route "/" serveSpa ]
      subRoute
          "/api"
          [ subRoute
              "/auth"
              [ POST [ routefdd "/authorize/%s" authorize
                       routed "/request" request ]
                GET [ route "/public" (text "public route") ]
                GET [ route "/secured" handleGetSecured ]
                |> applyBefore authorizeJwt ]
            subRoute
                "/pocket"
                [ GET [ routed "/retrieveAll" retrieveAll ]
                  |> applyBefore authorizeJwt ] ]
      |> applyBefore acceptJson ]