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

[<TestFixture>]
module ``templateToList tests`` =
    [<Test>]
    let ``1`` () =
        test <@ [ "d0" ] = templateToList "/qwe/{d0}" @>

    [<Test>]
    let ``2`` () =
        test <@ [ "d0"; "d1" ] = templateToList "/qwe/{d0}/{d1}/" @>

    [<Test>]
    let ``3`` () =
        test <@ [] = templateToList "/qwe/dasd/" @>

    [<Test>]
    let ``Empty`` () = test <@ [] = templateToList "" @>

    [<Test>]
    let ``Empty2`` () = test <@ [] = templateToList "/" @>

    [<Test>]
    let ``4`` () =
        test <@ [ "d0:long" ] = templateToList "/{d0:long}" @>

    [<Test>]
    let ``5`` () =
        test <@ [ "d0:long" ] = templateToList "/{d0:long}/" @>
