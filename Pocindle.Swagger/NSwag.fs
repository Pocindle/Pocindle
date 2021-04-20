module Pocindle.Swagger.NSwag

open NJsonSchema
open Newtonsoft.Json
open NSwag

open Newtonsoft.Json.Schema
open Pocindle.Swagger.Path
open Pocindle.Swagger.Utils

let pathToOpenApi path =
    let p = OpenApiPathItem()
    //p.Description <- $"%A{path}"

    let (Path (template, t, lst)) = path
    let ptypes = tupleTypeOptionToList t
    let parameterNames = templateToList template

    let parameters =
        (ptypes, parameterNames)
        ||> List.map2
                (fun t name ->
                    let par = OpenApiParameter()
                    par.Kind <- OpenApiParameterKind.Path
                    par.Name <- name
                    par.IsRequired <- true
                    par.Schema <- JsonSchema.FromType(t)
                    par)

    p.Parameters <- parameters |> List.toArray

    for verb, other in lst do
        let op = OpenApiOperation()
        let t = extractResponseTypes other

        for responseCode, tp in t do
            let response = OpenApiResponse()
            let mt = OpenApiMediaType()
            let s = Generation.JsonSchemaGeneratorSettings()
            s.SchemaType <- SchemaType.OpenApi3
            s.AllowReferencesWithProperties <- true
            mt.Schema <- JsonSchema.FromType(tp, s)
            response.Content.Add(ApplicationJson, mt)
            op.Responses.Add(responseCode |> string, response)

        p.Add(verb |> string, op)

    template |> string, p



let pathsToOpenApi paths = paths |> List.map pathToOpenApi

let operationsToOpenApi ops =
    let types = Operation.extractAllTypes ops
    let s = Generation.JsonSchemaGeneratorSettings()
    s.SchemaType <- SchemaType.OpenApi3
    s.AllowReferencesWithProperties <- true
    

    let schemas =
        types
        |> List.distinct
        |> List.map (fun tp -> tp.Name, JsonSchema.FromType(tp, s))

    let oa = OpenApiDocument()

    for a, i in schemas do
        oa.Components.Schemas.Add(a, i)

    let paths =
        ops |> operationsToPaths |> pathsToOpenApi

    for s, p in paths do
        oa.Paths.Add(s, p)

    oa

let toJson (doc: OpenApiDocument) =
    doc.ToJson(SchemaType.OpenApi3, Formatting.Indented)
