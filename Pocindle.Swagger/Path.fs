module Pocindle.Swagger.Path

open System
open Giraffe.EndpointRouting.Routers

open Pocindle.Swagger.Operation

type Path = Path of RouteTemplate * Type option * (HttpVerb * obj list) list

let operationsToPaths (ops: Operation list) =
    ops
    |> List.groupBy (fun (Operation (verb, template, t, meta)) -> template)
    |> List.map
        (fun (template, ops) ->
            let sops =
                ops
                |> List.map (fun (Operation (verb, template, t, meta)) -> verb, meta)

            let (Operation (_, _, t, _)) = ops |> List.head
            Path(template, t, sops))
