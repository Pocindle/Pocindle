module Pocindle.Playground.HJson

open NJsonSchema
open Pocindle.Pocket.Dto
open NJsonSchema.CodeGeneration.TypeScript

let tryNJson<'T> () =
    let schema = JsonSchema.FromType<'T>()
    let schemaData = schema.ToJson()

    let ts = TypeScriptGenerator(schema)

    let file = ts.GenerateFile()

    printfn $"%s{file}"
