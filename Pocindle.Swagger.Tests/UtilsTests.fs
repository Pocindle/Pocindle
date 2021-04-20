namespace Pocindle.Swagger.Tests.UtilsTests

open NUnit.Framework
open Swensen.Unquote

open Pocindle.Swagger.Utils

[<TestFixture>]
module ``tupleTypeToList tests`` =
    [<Test>]
    let ``1 string`` () =
        test <@ [ typeof<string> ] = tupleTypeToList typeof<string> @>

    [<Test>]
    let ``2 strings`` () =
        test <@ [ typeof<string>; typeof<string> ] = tupleTypeToList typeof<string * string> @>

    [<Test>]
    let ``Int and string`` () =
        test <@ [ typeof<int>; typeof<string> ] = tupleTypeToList typeof<int * string> @>

    [<Test>]
    let ``Unit`` () =
        test <@ [ typeof<unit> ] = tupleTypeToList typeof<unit> @>

[<TestFixture>]
module ``tupleTypeOptionToList tests`` =
    [<Test>]
    let ``1 string`` () =
        test <@ [ typeof<string> ] = tupleTypeOptionToList (Some typeof<string>) @>

    [<Test>]
    let ``2 strings`` () =
        test <@ [ typeof<string>; typeof<string> ] = tupleTypeOptionToList (Some typeof<string * string>) @>

    [<Test>]
    let ``Int and string`` () =
        test <@ [ typeof<int>; typeof<string> ] = tupleTypeOptionToList (Some typeof<int * string>) @>

    [<Test>]
    let ``Int and string vs string int`` () =
        test
            <@ [ typeof<int>; typeof<string> ]
               <> tupleTypeOptionToList (Some typeof<string * int>) @>

    [<Test>]
    let ``Unit`` () =
        test <@ [ typeof<unit> ] = tupleTypeOptionToList (Some typeof<unit>) @>

    [<Test>]
    let ``None`` () =
        test <@ [] = tupleTypeOptionToList None @>
