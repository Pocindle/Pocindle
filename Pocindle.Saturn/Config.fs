module Config

open Pocindle.Pocket.Common.SimpleTypes

type Config =
    { ConnectionString: string
      ConsumerKey: ConsumerKey }
