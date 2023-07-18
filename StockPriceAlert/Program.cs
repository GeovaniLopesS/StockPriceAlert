using StockPriceAlert;
using System.Globalization;
using System.Net;
using System.Text.Json;
class Program
{
    // salva o valor da última cotação recuperada
    private static double currentQuote = 0;
    private static string queryUrl;
    private static double stockPrice;
    public static void Main(string[] args)
    {
        // configura informaçoes do servidor smtp e api
        EmailSender.LoadConfig();
        SetQueryUrl(args[0]);

        // chama o metodo da API 
        while (true)
        {
            CheckStockQuote(args[0], double.Parse(args[1]), double.Parse(args[2]));

            Thread.Sleep(12000);
        }
    }

    private static void SetQueryUrl(string stock)
    {
        // carrega json de configuração da string
        string programDiretory = Directory.GetCurrentDirectory();
        JsonElement apiConfigJson = JsonDocument.Parse(File.ReadAllText(programDiretory + "\\apiconfig.json")).RootElement;

        queryUrl = apiConfigJson.GetProperty("url").GetString().Replace("{stock}", stock + ".SA").Replace("{apiKey}", 
            apiConfigJson.GetProperty("apiKey").GetString());
    }

    // Obtem o valor da ação no momento da consulta
    private static void CheckStockQuote(string stock, double upperBound, double lowerBound)
    {

        // definindo a url e instanciando um Uri 
        Uri queryUri = new Uri(queryUrl);
        using (WebClient client = new WebClient())
        {
            // realizando requisicao para a url e salvando como string
            string jsonData = client.DownloadString(queryUri);

            try
            {
                // obtendo o valor da ação a partir da chave"05. price"
                JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
                JsonElement jsonElement = jsonDocument.RootElement.GetProperty("Global Quote");
                
                //obtem o valor 
                stockPrice = GetStockPrice(jsonElement);
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);   
            }   

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
    }

    private static double GetStockPrice(JsonElement jsonElement)
    {
        try
        {
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