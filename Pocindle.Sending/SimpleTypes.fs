module Pocindle.Sending.SimpleTypes

open FSharp.UMX

[<Measure>]
type private smtpServer

type SmtpServer = string<smtpServer>

[<Measure>]
type private smtpPassword

type SmtpPassword = string<smtpPassword>
