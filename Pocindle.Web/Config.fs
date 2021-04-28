namespace Pocindle.Web

open System
open Microsoft.Extensions.Configuration

open Npgsql

open Pocindle.Domain.SimpleTypes

type Config =
    { ConnectionString: string
      ConsumerKey: ConsumerKey
      BaseUrl: Uri }

module Config =
    let buildConfig (ic: IConfiguration) =
        { ConsumerKey =
              ic.["ConsumerKey"]
              |> ConsumerKey.create
              |> Result.get
          ConnectionString =
              let builder =
                  NpgsqlConnectionStringBuilder(ic.GetConnectionString("DefaultConnection"))

              builder.Password <- ic.["DbPassword"]
              builder.ConnectionString
          BaseUrl = ic.["BaseUrl"] |> Uri }
