module Pocindle.Web.Router

open Microsoft.AspNetCore.Hosting

open Giraffe
open Giraffe.EndpointRouting

open Pocindle.Web.Auth
open Pocindle.Web.Pocket
open Pocindle.Web.Core
open Pocindle.Web.User
open Pocindle.Web.Convert
open Pocindle.Web.Delivery

let webApp =
    [ GET [ route "/openapi.json" (jsonFile "openapi.json") ]
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
                  |> applyBefore authorizeJwt ]
            subRoute
                "/user"
                [ GET [ routed "/" getUser ]
                  POST [ routefdd "/kindle-email/%s" setKindleEmailAddress ] ]
            |> applyBefore authorizeJwt
            subRoute "/convert" [ POST [ routefdd "/%s" convert ] ]
            |> applyBefore authorizeJwt
            subRoute "/delivery" [ GET [ routefdd "/%d" getDelivery ] ]
            |> applyBefore authorizeJwt ]
      |> applyBefore acceptJson ]
