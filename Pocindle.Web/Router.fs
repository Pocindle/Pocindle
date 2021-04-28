module Pocindle.Web.Router

open Microsoft.AspNetCore.Hosting

open Giraffe
open Giraffe.EndpointRouting

open Pocindle.Web.Template
open Pocindle.Web.Auth
open Pocindle.Web.Pocket

let serveSpa : HttpHandler =
    fun next ctx ->
        let c = ctx.GetService<Config>()

        match ctx.GetHostingEnvironment().IsDevelopment() with
        | true -> redirectTo false (string c.BaseUrl) next ctx
        | false -> htmlFile "index.html" next ctx

let acceptJson = mustAccept [ ApplicationJson ]

let routefd (path: PrintfFormat<_, _, _, _, 'T>) (routeHandler: 'T -> HttpHandler) : Endpoint =
    routef path routeHandler |> addMetadata typeof<'T>

let webApp =
    [ GET [ route "/" serveSpa
            route "/hello" (indexHandler ("world", "1"))
            routefd "/hello/%s/%s" indexHandler
            routefd "/hello/%s" indexHandler1 ]

      subRoute
          "/api"
          [ subRoute
              "/auth"
              [ POST [ routefd "/authorize/%s" authorize
                       route "/request" request ]
                GET [ route "/public" (text "public route") ]
                GET [ route "/secured" handleGetSecured ]
                |> applyBefore authorizeJwt ]
            subRoute
                "/pocket"
                [ GET [ route "/retrieveAll" retrieveAll ]
                  |> applyBefore authorizeJwt ] ]
      |> applyBefore acceptJson ]
