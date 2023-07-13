using StockPriceAlert;
using System.Net;
using System.Text.Json;
class Program
{
   public static void Main(string[] args)
    {

        // Aguarda 5 segundos entre cada chama do metodo de envio do email
        Timer timer = new Timer(CheckStockQuote, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        Console.ReadLine();
    }

    // Obtem o valor da ação no momento da consulta
    private static void CheckStockQuote(object state)
    {
       
        string QUERY_URL = "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=PETR4.SA&apikey=88YPN0WHXEHKRW30";
        Uri queryUri = new Uri(QUERY_URL);

        using (WebClient client = new WebClient())
        {
            string jsonData = client.DownloadString(queryUri);

            Console.WriteLine(jsonData);

            JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
            JsonElement jsonElement = jsonDocument.RootElement.GetProperty("Global Quote");
            
            string stockQuote = jsonElement.GetProperty("05. price").ToString();

            Console.WriteLine($"Stock quote: {stockQuote}");
        }
    }
}