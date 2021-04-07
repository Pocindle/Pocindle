module Router

open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters
open FSharp.Control.Tasks

open Pocindle.Saturn.Pocket
open Pocindle.Saturn

let browser =
    pipeline {
        plug acceptHtml
        plug putSecureBrowserHeaders
        plug fetchSession
        set_header "x-pipeline-type" "Browser"
    }

let defaultView =
    router {
        get "/index.html" (redirectTo false "/")
        get "/default.html" (redirectTo false "/")
        get "/" (htmlFile "index.html")
    }

let browserRouter =
    router {
        not_found_handler (htmlView NotFound.layout)
        pipe_through browser

        forward "" defaultView
    }

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

        forward "/auth" Auth.topRouter
        forward "/pocket" pocketApi
        forward "/someApi" someScopeOrController
    }

let appRouter =
    router {
        forward "/api" apiRouter
        forward "" browserRouter
    }
