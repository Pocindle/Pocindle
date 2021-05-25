module Pocindle.Sending.SendMobiDelivery

open System.IO
open System.Net
open System.Net.Mail

open System.Threading
open FsToolkit.ErrorHandling
open FSharp.Control.Tasks
open FSharp.UMX

open Pocindle.Convert.Domain
open Pocindle.Domain
open Pocindle.Domain.SimpleTypes
open Pocindle.Sending.SimpleTypes

let send (host: SmtpServer) (from: MailAddress) (to': MailAddress) (password: SmtpPassword) (MobiFile filename) =
    taskResult {
        try
            use smtpClient =
                new SmtpClient(
                    UseDefaultCredentials = false,
                    Credentials = NetworkCredential(from.Address, %password),
                    Host = %host,
                    Port = 587,
                    EnableSsl = true
                )

            use message =
                new MailMessage(from, to', Subject = $"Delivery {filename}")

            use sr = new StreamReader(filename)

            use attachment = new Attachment(sr.BaseStream, filename)

            message.Attachments.Add attachment

            return! smtpClient.SendMailAsync(message)
        with ex -> return! Error ex
    }
