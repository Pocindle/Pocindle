module internal Pocindle.Pocket.Auth.Implementation

open System
open System.Net.Http
open System.Text

open FSharp.UMX
open FSharp.Control.Tasks
open FsToolkit.ErrorHandling

open Pocindle.Pocket.Auth.PocketDto
open Pocindle.Pocket.Auth.PublicTypes
open Pocindle.Pocket.Auth.SimpleTypes
open Pocindle.Domain.SimpleTypes
open Pocindle.Common.Serialization

let pocketSendRetrieve<'Request, 'Response> (request: 'Request) (uri: Uri) =
    taskResult {
        let! json =
            serialize request
            |> Result.mapError SerializationError

        let content =
            new StringContent(json, Encoding.UTF8, ApplicationJson)

        use httpClient = new HttpClient()
        content.Headers.Add(XAccept, ApplicationJson)

        let! res1 =
            task {
                try
                    let! response = httpClient.PostAsync(uri, content)

                    let! res = response.Content.ReadAsStringAsync()

                    return
                        deserialize<'Response> res
                        |> Result.mapError DeserializationError
                with ex -> return Error ^ Exception ex
            }

        return res1
    }

let obtainRequestToken : ObtainRequestToken =
    fun (consumer_key: ConsumerKey) (redirect_uri: PocindleRedirectPrefix) (state: State) ->
        taskResult {
            let req =
                ObtainRequestTokenRequestDto.fromDomain consumer_key redirect_uri state

            let! res1 =
                pocketSendRetrieve<ObtainRequestTokenRequestDto, ObtainRequestTokenResponseDto>
                    req
                    (Uri("https://getpocket.com/v3/oauth/request", UriKind.Absolute))

            let stte : State =
                res1.state |> Option.ofObj |> Option.map (~%)

            let! rt =
                RequestToken.create res1.code
                |> Result.mapError ParseError

            return (rt, stte)
        }

let authorize (consumer_key: ConsumerKey) (code: RequestToken) =
    taskResult {
        let req =
            AuthorizeRequestDto.fromDomain consumer_key code

        let! res1 =
            pocketSendRetrieve<AuthorizeRequestDto, AuthorizeResponseDto>
                req
                (Uri("https://getpocket.com/v3/oauth/authorize", UriKind.Absolute))

        let! rt =
            AccessToken.create res1.access_token
            |> Result.mapError ParseError

        let! user =
            PocketUsername.create res1.username
            |> Result.mapError ParseError

        return (rt, user)
    }
