using StockPriceAlert;
using System.Globalization;
using System.Net;
using System.Text.Json;
class Program
{
    // salva o valor da última cotação recuperada
    private static decimal currentQuote { get; set; }

    public static void Main(string[] args)
    {
        currentQuote = 0;

        // chama o metodo da API 
        while (true)
        {
            CheckStockQuote(args[0], decimal.Parse(args[1]), decimal.Parse(args[2]));
            Thread.Sleep(5000);
        }
    }

    // Obtem o valor da ação no momento da consulta
    private static void CheckStockQuote(string stock, decimal upperBound, decimal lowerBound)
    {  
        string url = "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=";
        string apiKey = "&apikey=88YPN0WHXEHKRW30";

        // definindo a url e instanciando um Uri 
        string QUERY_URL = url + stock + ".SA" + apiKey;
        Uri queryUri = new Uri(QUERY_URL);

        using (WebClient client = new WebClient())
        {
            // realizando requisicao para a url e salvando como string
            string jsonData = client.DownloadString(queryUri);
            Console.Write(jsonData);

            // obtendo o valor da ação a partir da chave"05. price"
            JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
            try
            {
                JsonElement jsonElement = jsonDocument.RootElement.GetProperty("Global Quote");
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);   
            }   

            string stockPrice = jsonElement.GetProperty("05. price").GetString();    
            decimal stockQuote = Convert.ToDecimal(stockPrice, CultureInfo.InvariantCulture);

            //verifica se o valor encontrado é diferente do último valor verificado
            if (stockQuote != currentQuote)
            {
                // verifica se a cotação atingiu algum limite e envia o email
                if (stockQuote >= upperBound)
                {
                    string body = "A ação " + stock + " atingiu R$" + stockQuote + ". venda!";
                    EmailSender.SendEmail(body);

                }
                else if (stockQuote <= lowerBound)
                {
                    string body = "A ação " + stock + " atingiu R$" + stockQuote + ". compre!";
                    EmailSender.SendEmail(body);

                }
                Console.WriteLine("Email enviado!");
                //salva o valor da cotação para controle
                currentQuote = stockQuote;
            }
            // se o valor se manteve, então não envia email.
            else
            {
                return;
            }
        }
    }
}