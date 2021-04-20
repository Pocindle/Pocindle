namespace Pocindle.Pocket.Pocket

open System.Threading.Tasks
open Pocindle.Pocket.Auth.PublicTypes
open Pocindle.Pocket.Auth.SimpleTypes
open Pocindle.Pocket.Common.SimpleTypes
open Pocindle.Pocket.Retrieve.PublicTypes
open Pocindle.Domain.SimpleTypes

module Builder =
    open Pocindle.Pocket.Auth.SimpleTypes

    type FreePocketInstruction<'a> =
        | OAuthRequest of (ConsumerKey * RedirectUri * State * (Task<Result<RequestToken * State, AuthError>> -> 'a))
        | OAuthAuthorize of (ConsumerKey * RequestToken * (Task<Result<AccessToken * PocketUsername, AuthError>> -> 'a))
        | Retrieve of
            (ConsumerKey * AccessToken * RetrieveOptionalParameters * (Task<Result<RetrieveResponse, RetrieveError>> -> 'a))

    let private mapI f =
        function
        | OAuthRequest (a, b, c, next) -> OAuthRequest(a, b, c, next >> f)
        | OAuthAuthorize (a, b, next) -> OAuthAuthorize(a, b, next >> f)
        | Retrieve (a, b, c, next) -> Retrieve(a, b, c, next >> f)

    type FreePocketProgram<'a> =
        | Free of FreePocketInstruction<FreePocketProgram<'a>>
        | Pure of 'a

    let rec bind f =
        function
        | Free x -> x |> mapI (bind f) |> Free
        | Pure x -> f x

    type FreePocketBuilder() =
        member this.Bind(x, f) = bind f x
        member this.Return x = Pure x
        member this.ReturnFrom x = x
        member this.Zero() = Pure()

    let interpret imp1 imp2 imp3 =
        let rec recInterpret =
            function
            | Pure x -> x
            | Free (OAuthRequest (a, b, c, next)) -> (a, b, c) |||> imp1 |> next |> recInterpret
            | Free (OAuthAuthorize (a, b, next)) -> (a, b) ||> imp2 |> next |> recInterpret
            | Free (Retrieve (a, b, c, next)) -> (a, b, c) |||> imp3 |> next |> recInterpret

        recInterpret

module CE =
    open Builder

    module Pocket =
        let oAuthRequest a b c = Free(OAuthRequest(a, b, c, Pure))
        let oAuthAuthorize a b c = Free(OAuthAuthorize(a, b, Pure))
        let retrieve a b c = Free(Retrieve(a, b, c, Pure))

    let freePocket = FreePocketBuilder()
