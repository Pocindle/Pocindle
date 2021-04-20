module Pocindle.Swagger.Operation

open System
open Giraffe.EndpointRouting.Routers

open Pocindle.Swagger.Utils

type Operation = Operation of HttpVerb * RouteTemplate * Type option * obj list

let rec endpointToOperations routeTemplate m1 endpoint =
    match endpoint with
    | SimpleEndpoint (verb, template, handler, meta) ->
        let t, m = extractType meta

        Operation(verb, routeTemplate + template, t, m1 @ m)
        |> List.singleton
    | TemplateEndpoint (verb, template, templateMappings, handler, meta) ->
        let t, m = extractType meta

        Operation(verb, routeTemplate + template, t, m1 @ m)
        |> List.singleton
    | NestedEndpoint (template, endpoints, meta) ->
        endpoints
        |> List.collect (endpointToOperations (routeTemplate + template) (m1 @ meta))
    | MultiEndpoint endpoints ->
        endpoints
        |> List.collect (endpointToOperations routeTemplate m1)

let endpointsToOperations endpoints =
    endpoints
    |> List.collect (endpointToOperations "" [])

let extractAllTypes ops =
    ops
    |> List.collect
        (fun (Operation (_, _, t, o)) ->
            (t |> tupleTypeOptionToList)
            @ (extractResponseTypes o |> List.map snd))
