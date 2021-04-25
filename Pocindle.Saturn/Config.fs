namespace Pocindle.Saturn

open Saturn

open Pocindle.Pocket.Common.SimpleTypes

type Config =
    { ConnectionString: string
      ConsumerKey: ConsumerKey }

module Config =
    let getConfig ctx : Config = Controller.getConfig ctx
