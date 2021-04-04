module Router

open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters
open FSharp.Control.Tasks


let browser =
    pipeline {
        plug acceptHtml
        plug putSecureBrowserHeaders
        plug fetchSession
        set_header "x-pipeline-type" "Browser"
    }

let defaultView =
    router {
        //get "/" (htmlView Index.layout)
        get "/index.html" (redirectTo false "/")
        get "/default.html" (redirectTo false "/")
        get "/" (htmlFile "index.html")
    }

let browserRouter =
    router {
        not_found_handler (htmlView NotFound.layout) //Use the default 404 webpage
        pipe_through browser //Use the default browser pipeline

        forward "" defaultView //Use the default view
    }

//Other scopes may use different pipelines and error handlers

let api =
    pipeline {
        plug acceptJson
        set_header "x-pipeline-type" "Api"
    }

let someScopeOrController =
    router {
        getf
            "/short/%s/%s"
            (fun (i, j) func ctx ->
                task {
                    Controller.getConfig ctx |> printfn "%A"

                    let! r = json (sprintf "%s short" i) func ctx
                    return r
                })

        not_found_handler (text "Not Found")
    }

let apiRouter =
    router {
        not_found_handler (text "Api 404")
        pipe_through api

        forward "/someApi" someScopeOrController
    }

let appRouter =
    router {
        forward "/api" apiRouter
        forward "" browserRouter
    }
