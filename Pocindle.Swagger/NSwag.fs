module Pocindle.Swagger.NSwag

open NJsonSchema
open Newtonsoft.Json
open NSwag

open Newtonsoft.Json.Schema
open Pocindle.Swagger.Path
open Pocindle.Swagger.Utils


let operationsToOpenApi ops =
    let types = Operation.extractAllTypes ops
    let s = Generation.JsonSchemaGeneratorSettings()
    s.SchemaType <- SchemaType.OpenApi3
    //s.AllowReferencesWithProperties <- true

    let schemasList =
        types
        |> List.distinct
        |> List.map (fun tp -> tp, JsonSchema.FromType(tp, s))

    let schemas = schemasList |> dict

    let s1 =
        schemasList
        |> List.collect
            (fun (t, schema) ->
                schema.Definitions
                |> Seq.map (|KeyValue|)
                |> Seq.toList)

    schemasList
    |> List.iter (fun (_, schema) -> schema.Definitions.Clear())

    let document = OpenApiDocument()
    document.SchemaType <- SchemaType.OpenApi3

    for a, i in schemas |> Seq.map (|KeyValue|) do
        document.Components.Schemas.[a.Name] <- i

    for a, i in s1 do
        document.Components.Schemas.[a] <- i

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
                        //par.Reference <- schemas.[t]
                        par)

        p.Parameters <- parameters |> List.toArray

        for verb, other in lst do
            let op = OpenApiOperation()
            let t = extractResponseTypes other

            match t with
            | [] -> op.Responses.["200"] <- OpenApiResponse()
            | _ ->
                for responseCode, tp in t do
                    //response.Reference <- OpenApiResponse(Schema = schemas.[tp])
                    //response.Schema <- schemas.[tp]
                    
                    let response = OpenApiResponse()
                    response.Content.[ApplicationJson] <-OpenApiMediaType(Schema = schemas.[tp])
                    op.Responses.[responseCode |> string] <- response

            p.Add(verb |> string, op)

        template |> string, p

    let paths = ops |> operationsToPaths

    let op = paths |> List.map pathToOpenApi

    for s, p in op do
        document.Paths.[s] <- p

    document

let toJson (doc: OpenApiDocument) =
    doc.ToJson(SchemaType.OpenApi3, Formatting.Indented)
