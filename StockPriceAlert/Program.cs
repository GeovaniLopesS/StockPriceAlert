using StockPriceAlert;
using System.Globalization;
using System.Net;
using System.Text.Json;
class Program
{
    // salva o valor da última cotação recuperada
    private static double currentQuote = 0;


    public static void Main(string[] args)
    {
        // configura informaçoes do servidor smtp e api
        EmailSender.LoadConfig();

        // chama o metodo da API 
        while (true)
        {
            CheckStockQuote(args[0], double.Parse(args[1]), double.Parse(args[2]));
            
            Thread.Sleep(5000);
        }
    }

    // Obtem o valor da ação no momento da consulta
    private static void CheckStockQuote(string stock, double upperBound, double lowerBound)
    {
        string jsonData = ApiFetcher.GetStockData(stock);

        double stockPrice = GetStockPrice(jsonData);

        //verifica se o valor encontrado é diferente do último valor verificado
        if (stockPrice != currentQuote)
        {
            // verifica se a cotação atingiu algum limite e envia o email
            if (stockPrice >= upperBound)
            {
                string body = "A ação " + stock + " atingiu R$" + stockPrice + ". venda!";
                EmailSender.SendEmail(stock, body);

            }
            else if (stockPrice <= lowerBound)
            {
                string body = "A ação " + stock + " atingiu R$" + stockPrice + ". compre!";
                EmailSender.SendEmail(stock, body);

            }

            //salva o valor da cotação para controle
            currentQuote = stockPrice;
        }
        // se o valor se manteve, então não envia email.
        else
        {
            return;
        }

    }
    private static double GetStockPrice(string jsonData)
    {
        try
        {
            // obtendo o valor da ação a partir da chave"05. price"
            JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
            JsonElement jsonElement = jsonDocument.RootElement.GetProperty("Global Quote");
            string stockPricestring = jsonElement.GetProperty("05. price").GetString();
            double price = Convert.ToDouble(stockPricestring, CultureInfo.InvariantCulture);
            return price;
        }
        catch (Exception e)
        { 
            return 0;
        }
    }
}