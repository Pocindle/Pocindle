open System
open System.IO
open System.Net
open System.Text.Json
open System.Net.Mail

open FSharp.UMX
open FSharp.Control.Tasks
open FsToolkit.ErrorHandling

open Pocindle.Convert.Domain
open Pocindle.Convert.Api

let zero x = 0

let min x = if x >= 0 then x - 1 else zero x

let rec map mathFunc =
    fun x y result ->
        if y = zero x then
            result
        else
            map mathFunc x (min y) (mathFunc result x)

let multiplication = map (fun result x -> result + x)

let power =
    map (fun result x -> multiplication result x 0)

let superpower x = power x x


let sendEmail (password: string) (filename: string) =
    task {
        use smtpClient =
            new SmtpClient(
                "smtp.yandex.ru",
                Port = 587,
                Credentials = NetworkCredential("misterptits@yandex.ru", password),
                EnableSsl = true
            )

        use message =
            new MailMessage("misterptits@yandex.ru", "misterptits@ya.ru", Subject = $"Delivery {filename}")

        use sr = new StreamReader(filename)

        use attachment = new Attachment(sr.BaseStream, filename)

        message.Attachments.Add attachment

        return! smtpClient.SendMailAsync(message)
    }

[<EntryPoint>]
let main args =
    if args.Length = 1 then
        sendEmail args.[0] "1cd6f9cf-0902-4ad4-a721-0e2323e4a890.epub"
        |> Async.AwaitTask
        |> Async.RunSynchronously
    else
        let epub =
            convertWebToEpub
                Pandoc
                (Article(
                    Uri(
                        "https://lexi-lambda.github.io/blog/2020/01/19/no-dynamic-type-systems-are-not-inherently-more-open/"
                    )
                ))
            |> Async.AwaitTask
            |> Async.RunSynchronously

        match epub with
        | Ok epub ->
            let mobi =
                convertEpubToMobi Calible epub
                |> Async.AwaitTask
                |> Async.RunSynchronously

            printfn $"%A{mobi}"
        | Error error -> raise500 error

    0
