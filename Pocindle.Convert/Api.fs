module Pocindle.Convert.Api

open Pocindle.Convert.Domain

let convertWebToEpub : ConvertWebToEpub = Implementation.convertToEpub

let convertEpubToMobi : ConvertEpubToMobi = Implementation.convertToMobi
