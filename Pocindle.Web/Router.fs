module Pocindle.Web.Router

open Microsoft.AspNetCore.Hosting

open Giraffe
open Giraffe.EndpointRouting

open Pocindle.Web.Template

let serveSpa : HttpHandler =
    fun next ctx ->
        let c = ctx.GetService<Config>()

        match ctx.GetHostingEnvironment().IsDevelopment() with
        | true -> redirectTo false (string c.BaseUrl) next ctx
        | false -> htmlFile "index.html" next ctx
        
let webApp =    
    [ GET [ route "/" serveSpa
            route "/hello" (indexHandler ("world","1"))
            routef "/hello/%s/%s" indexHandler
            routef "/hello/%s" indexHandler1
            subRoute "" [ route "" (text "") ]
            |> applyBefore (text "") ] ]
