using System;
using System.Text.Json;
using MailKit.Net.Smtp;
using MimeKit;


namespace StockPriceAlert
{
    class EmailSender
    {   
        // credenciais do email que envia a mensagem
        private static string senderEmail;
        private static string senderPassword;

        // configuraçoes do servidor smtp 
        // email que recebe a mensagem
        private static string reciever;
        private static string smtpHost;
        private static int smtpPort;
        private static bool useSsl;

        public static void SendEmail(string body)
        {
            // define o remetente, destinatario, titulo e corpo do email
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Tester Remetente", senderEmail));
            message.To.Add(new MailboxAddress("Tester Destinatario", reciever));
            message.Subject = "Atenção!";
            message.Body = new TextPart("plain")

            {
                Text = body
            };

            // instancia um smtp client para enviar o email
            SmtpClient client = new SmtpClient();
            try
            {
                client.Connect(smtpHost, 
                    smtpPort, 
                    useSsl);

                client.Authenticate(senderEmail, senderPassword);
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            client.Disconnect(true);
        }

        public static void LoadConfig()
        {
            // lendo arquivo json com as configurações do servidor smtp
            string programDiretory = System.IO.Directory.GetCurrentDirectory();
            JsonDocument serverConfig = JsonDocument.Parse(File.ReadAllText(programDiretory + "\\serverconfig.json"));
            JsonElement serverConfigElement = serverConfig.RootElement;

            // carrega valores do servidor smtp
            smtpHost = serverConfigElement.GetProperty("host").GetString();
            smtpPort = serverConfigElement.GetProperty("port").GetInt16();
            useSsl = serverConfigElement.GetProperty("useSsl").GetBoolean();
            reciever = serverConfigElement.GetProperty("reciever").GetString();

            // recuperando informação das credenciais do email que enviará
            senderEmail = Environment.GetEnvironmentVariable("appEmail");
            senderPassword = Environment.GetEnvironmentVariable("appPassword");
        }
    }
}