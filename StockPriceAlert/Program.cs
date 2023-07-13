using StockPriceAlert;
using System.Net;
using System.Text.Json;
class Program
{
    public static void Main(string[] args)
    {
        // chama o metodo da API 
        while (true)
        {
            CheckStockQuote(float.Parse(args[0]), float.Parse(args[1]));
            Thread.Sleep(5000);
        }
    }

    // Obtem o valor da ação no momento da consulta
    private static void CheckStockQuote(float upperBound, float lowerBound)
    {      
       // definindo a url e instanciando um Uri 
        string QUERY_URL = "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=PETR4.SA&apikey=88YPN0WHXEHKRW30";
        Uri queryUri = new Uri(QUERY_URL);

        using (WebClient client = new WebClient())
        {
            // realizando requisicao para a url e salvando como string
            string jsonData = client.DownloadString(queryUri);
            Console.WriteLine(jsonData);

            // obtendo o valor da ação a partir das chaves "Global Quote" e "05. price"
            JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
            JsonElement jsonElement = jsonDocument.RootElement.GetProperty("Global Quote");
            string stockQuote = jsonElement.GetProperty("05. price").ToString();

            Console.WriteLine($"Stock quote: {stockQuote}");
        }
    }
}