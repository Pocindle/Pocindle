module Pocindle.Swagger.Operation

open System
open Giraffe.EndpointRouting.Routers

type Operation = Operation of HttpVerb * RouteTemplate * Type option * obj list

let extractType meta =
    let t, other =
        meta
        |> List.partition
            (fun (t: obj) ->
                match t with
                | :? Type -> true
                | _ -> false)

    t
    |> List.tryHead
    |> Option.map (fun o -> o :?> Type),
    other

let rec endpointToOperations routeTemplate endpoint =
    match endpoint with
    | SimpleEndpoint (verb, template, handler, meta) ->
        let t, m = extractType meta

        Operation(verb, routeTemplate + template, t, m)
        |> List.singleton
    | TemplateEndpoint (verb, template, templateMappings, handler, meta) ->
        let t, m = extractType meta

        Operation(verb, routeTemplate + template, t, m)
        |> List.singleton
    | NestedEndpoint (template, endpoints, meta) ->
        endpoints
        |> List.collect (endpointToOperations (routeTemplate + template))
    | MultiEndpoint endpoints ->
        endpoints
        |> List.collect (endpointToOperations routeTemplate)

let endpointsToOperations endpoints =
    endpoints
    |> List.collect (endpointToOperations "")
