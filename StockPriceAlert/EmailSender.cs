using System;
using System.Text.Json;
using MailKit.Net.Smtp;
using MimeKit;
using static System.Net.Mime.MediaTypeNames;

namespace StockPriceAlert
{
    class EmailSender
    {
        public static void SendEmail(string body)
        {
            // lendo arquivo json com as configurações do servidor smtp
            JsonDocument serverConfig = JsonDocument.Parse(File.ReadAllText("C:\\Users\\Geovani\\source\\repos\\StockPriceAlert\\StockPriceAlert\\serverconfig.json"));
            JsonElement serverConfigElement = serverConfig.RootElement;

            // recuperando informação das credenciais do email que enviará
            string senderEmail = Environment.GetEnvironmentVariable("appEmail");
            string senderPassword = Environment.GetEnvironmentVariable("appPassword");

            Console.WriteLine(senderEmail + " " + senderPassword);

            // define o remetente, destinatario, titulo e corpo do email
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Tester Remetente", senderEmail));
            message.To.Add(new MailboxAddress("Tester Destinatario", serverConfigElement.GetProperty("reciever").GetString()));
            message.Subject = "Atenção!";
            message.Body = new TextPart("plain")
            {
                Text = body
            };

            // instancia um smtp client para enviar o email
            SmtpClient client = new SmtpClient();

            try
            {
                client.Connect(serverConfigElement.GetProperty("host").GetString(), 
                    serverConfigElement.GetProperty("port").GetInt16(), 
                    serverConfigElement.GetProperty("useSsl").GetBoolean());
                client.Authenticate(senderEmail, "tyfqlotifdoulizp");
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            client.Disconnect(true);
        }
    }
}