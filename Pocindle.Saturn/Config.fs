namespace Pocindle.Saturn

open System
open Saturn

open Pocindle.Domain.SimpleTypes

type Config =
    { ConnectionString: string
      ConsumerKey: ConsumerKey
      BaseUrl: Uri }

module Config =
    let getConfig ctx : Config = Controller.getConfig ctx

