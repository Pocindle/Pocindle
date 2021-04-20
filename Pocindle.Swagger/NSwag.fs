module Pocindle.Swagger.NSwag

open NSwag

open Pocindle.Swagger.Operation

let operationToOpenApiOperation operation =
    let o = OpenApiOperation()
    unimplemented ""