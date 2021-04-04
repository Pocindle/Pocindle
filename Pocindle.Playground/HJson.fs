module Pocindle.Playground.HJson

open NJsonSchema
open Pocindle.Pocket.Dto
open NJsonSchema.CodeGeneration.TypeScript

let tryNJson () =
    let schema = JsonSchema.FromType<Dto>()
    let schemaData = schema.ToJson()

    let ts = TypeScriptGenerator(schema)

    let file = ts.GenerateFile()

    printfn $"%s{file}"
