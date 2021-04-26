namespace Pocindle.Saturn

open System
open Saturn

open Pocindle.Pocket.Common.SimpleTypes

type Config =
    { ConnectionString: string
      ConsumerKey: ConsumerKey
      BaseUrl: Uri }

module Config =
    let getConfig ctx : Config = Controller.getConfig ctx
