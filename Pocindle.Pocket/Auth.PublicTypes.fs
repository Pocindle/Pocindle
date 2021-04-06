module Pocindle.Pocket.Auth.PublicTypes

open System
open System.Net.Http
open System.Text
open System.Text.Json
open System.Threading.Tasks

open FSharp.UMX
open FSharp.Control.Tasks
open FsToolkit.ErrorHandling

open Pocindle.Domain.SimpleTypes
open Pocindle.Pocket.Auth.SimpleTypes
open Pocindle.Pocket.Common.SimpleTypes

type AuthError =
    | Exception of exn
    | ValidationError
    | ParseError of string


type ObtainRequestToken = ConsumerKey -> RedirectUri -> State -> Task<Result<RequestToken * State, AuthError>>
