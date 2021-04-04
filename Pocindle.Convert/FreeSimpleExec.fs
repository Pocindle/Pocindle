namespace Pocindle.Convert

open System.Threading.Tasks
open FsToolkit.ErrorHandling

module FreeSimpleExec =
    type SimpleExecInstruction<'a> =
        | SimpleExecAsync of (string * (Task<Result<unit, exn>> -> 'a))
        | SimpleExec of (string * (Result<unit, exn> -> 'a))

    let private mapI f =
        function
        | SimpleExecAsync (x, next) -> SimpleExecAsync(x, next >> f)
        | SimpleExec (x, next) -> SimpleExec(x, next >> f)

    type SimpleExecProgram<'a> =
        | Free of SimpleExecInstruction<SimpleExecProgram<'a>>
        | Pure of 'a

    let rec bind f =
        function
        | Free x -> x |> mapI (bind f) |> Free
        | Pure x -> f x

    type SimpleExecBuilder() =
        member this.Bind(x, f) = bind f x
        member this.Return x = Pure x
        member this.ReturnFrom x = x
        member this.Zero() = Pure()


    let simpleExecAsync in1 = Free(SimpleExecAsync(in1, Pure))

    let simpleExec' in2 = Free(SimpleExec(in2, Pure))

    open SimpleExec

    let imp1 a =
        taskResult {
            try
                return! Command.RunAsync(a)
            with ex -> return! Error ex
        }

    let imp2 a =
        result {
            try
                return Command.Run(a)
            with ex -> return! Error ex
        }

    let rec interpret =
        function
        | Pure x -> x
        | Free (SimpleExecAsync (x, next)) -> x |> imp1 |> next |> interpret
        | Free (SimpleExec (x, next)) -> x |> imp2 |> next |> interpret

    let rec interpretStub out1 out2 =
        function
        | Pure x -> x
        | Free (SimpleExecAsync (_, next)) -> out1 |> next |> interpretStub out1 out2
        | Free (SimpleExec (_, next)) -> out2 |> next |> interpretStub out1 out2


module FreeSimpleExecCE =
    let simpleExec = FreeSimpleExec.SimpleExecBuilder()
