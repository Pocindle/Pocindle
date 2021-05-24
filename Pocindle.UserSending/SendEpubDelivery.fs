module Pocindle.SendEpubDelivery

open System.IO
open System.Net
open System.Net.Mail

open FsToolkit.ErrorHandling
open FSharp.Control.Tasks

open Pocindle.Convert.Domain
open Pocindle.Domain
open Pocindle.Domain.SimpleTypes

let send (deliveryConfig: DeliveryConfig) (EpubFile epub) =
    taskResult {
        try
            let host, port =
                SmtpServer.fromDomain deliveryConfig.SmtpServer

            use smtpClient =
                new SmtpClient(
                    host,
                    Port = port,
                    Credentials = NetworkCredential(deliveryConfig.From.Address, deliveryConfig.Password),
                    EnableSsl = true
                )

            use message =
                new MailMessage(deliveryConfig.From, deliveryConfig.To, Subject = $"Delivery {epub}")

            use sr = new StreamReader(epub)

            use attachment = new Attachment(sr.BaseStream, epub)

            message.Attachments.Add attachment

            return! Ok ^ smtpClient.SendMailAsync(message)
        with ex -> return! Error ex
    }
