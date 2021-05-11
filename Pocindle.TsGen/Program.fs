open System
open System.IO
open System.Threading.Tasks

open FsToolkit.ErrorHandling
open NJsonSchema
open NJsonSchema.CodeGeneration.TypeScript
open Newtonsoft.Json

let generateSchemaAndTs<'T> () =
    taskResult {
        try
            let schema = JsonSchema.FromType<'T>()
            let ts = TypeScriptGenerator(schema)
            return schema, ts.GenerateFile()
        with ex -> return! Error(ex, typeof<'T>)
    }

let toFilename (schema: JsonSchema) =
    $"%c{Char.ToLowerInvariant(schema.Title.[0])}%s{(schema.Title.Substring(1))}.ts"


let generateOpenApi () =
    let ops =
        Pocindle.Swagger.Operation.endpointsToOperations Pocindle.Web.Router.webApp

    let doc =
        Pocindle.Swagger.NSwag.operationsToOpenApi ops

    use sw =
        new StreamWriter(Path.Combine(__SOURCE_DIRECTORY__, "../Pocindle.Web/openapi.json"))

    sw.WriteLine(doc.ToJson(SchemaType.OpenApi3, Formatting.Indented))

let generateTs () =
    let schemas =
        [ generateSchemaAndTs<Pocindle.Pocket.Retrieve.Dto.PocketRetrieveDto> ()
          generateSchemaAndTs<Pocindle.Domain.Dto.DeliveryDto> ()
          generateSchemaAndTs<Pocindle.Pocket.Auth.Dto.RequestDto> ()
          generateSchemaAndTs<Pocindle.Web.Auth.JwtTokenDto> () ]

    let dtoPath =
        Path.Combine(__SOURCE_DIRECTORY__, "../pocindle-client/src/api/dto/")

    schemas
    |> List.map
        (fun task ->
            taskResult {
                let! schema, file = task
                let filename = Path.Combine(dtoPath, toFilename schema)
                use sw = new StreamWriter(filename)
                do! sw.WriteLineAsync(file)
                return filename
            })
    |> List.map Async.AwaitTask
    |> Async.Parallel
    |> Async.RunSynchronously
    |> Array.iter
        (function
        | Ok fn -> printfn $"Готово: %s{fn}"
        | Error (ex, type') -> eprintfn $"Ошибка при %A{type'}:\n%A{ex}")

[<EntryPoint>]
let main _ =
    generateOpenApi ()
    generateTs ()
    0
