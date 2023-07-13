using System;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace StockPriceAlert
{
    class EmailSender
    {
        public static void SendEmail(string remetente, string destinatario, string result)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Tester Remetente", remetente));
            message.To.Add(new MailboxAddress("Tester Destinatario", destinatario));
            message.Subject = "Atenção!";

            message.Body = new TextPart("plain")
            {
                Text = @"Olá, isso é um teste3. " + result
            };

            SmtpClient client = new SmtpClient();

            try
            {
                client.Connect("smtp.gmail.com", 587, false);

                client.Authenticate("stockquotealert1@gmail.com", "tyfqlotifdoulizp");

                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro!");
                Console.WriteLine(ex.ToString());
            }
            Console.ReadLine();
            client.Disconnect(true);

        }
    }
}