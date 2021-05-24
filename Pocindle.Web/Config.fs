namespace Pocindle.Web

open System
open System.Net.Mail
open Microsoft.Extensions.Configuration

open Npgsql
open FsToolkit.ErrorHandling
open FSharp.UMX

open Pocindle.Domain.SimpleTypes
open Pocindle.Database
open Pocindle.Domain
open Pocindle.Sending.SimpleTypes

[<Measure>]
type private jwtIssuer

type JwtIssuer = string<jwtIssuer>

[<Measure>]
type private jwtSecret

type JwtSecret = string<jwtSecret>

type Config =
    { ConnectionString: ConnectionString
      ConsumerKey: ConsumerKey
      SpaUrl: SpaUrl
      JwtIssuer: JwtIssuer
      JwtSecret: JwtSecret
      SmtpServer: SmtpServer
      SmtpPassword: SmtpPassword
      SmtpSenderEmail: MailAddress }

module Config =
    let buildConfig (ic: IConfiguration) =
        result {
            let! consumerKey = ic.["ConsumerKey"] |> ConsumerKey.create

            let! connectionString =
                match ic.GetConnectionString("DefaultConnection"), ic.["DbPassword"] with
                | null, _ -> Error "ConnectionString('DefaultConnection') is not set"
                | _, null -> Error "DbPassword is not set"
                | connStr, password ->
                    let builder = NpgsqlConnectionStringBuilder(connStr)

                    builder.Password <- password
                    Ok %builder.ConnectionString

            let! spaUrl =
                match ic.["SpaUrl"] with
                | null -> Error "SpaUrl is not set"
                | b -> SpaUrl.fromString b |> Ok

            let! jwtIssuer =
                match ic.["JwtIssuer"] with
                | null -> Error "JwtIssuer is not set"
                | b -> Ok %b

            let! jwtSecret =
                match ic.["JwtSecret"] with
                | null -> Error "JwtSecret is not set"
                | b -> Ok %b

            let! smtpServer =
                match ic.["SmtpServer"] with
                | null -> Error "SmtpServer is not set"
                | b -> Ok %b

            let! smtpPassword =
                match ic.["SmtpPassword"] with
                | null -> Error "SmtpPassword is not set"
                | b -> Ok %b

            let! smtpSenderEmail =
                match ic.["SmtpSenderEmail"] with
                | null -> Error "SmtpSenderEmail is not set"
                | b -> Ok %b

            return
                { ConsumerKey = consumerKey
                  ConnectionString = connectionString
                  SpaUrl = spaUrl
                  JwtIssuer = jwtIssuer
                  JwtSecret = jwtSecret
                  SmtpServer = smtpServer
                  SmtpPassword = smtpPassword
                  SmtpSenderEmail = MailAddress(smtpSenderEmail) }
        }
