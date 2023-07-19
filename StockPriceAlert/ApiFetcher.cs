using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockPriceAlert
{
    static class ApiFetcher
    {
        private static string queryUrl;
        private static string jsonData;

        static ApiFetcher()
        {
            SetQueryUrl();
        }
        public static string GetStockData(string stock)
        {
            // definindo a url e instanciando um Uri 
            Uri queryUri = new Uri(queryUrl.Replace("{stock}", stock + ".SA"));

            using (WebClient client = new WebClient())
            {
                // realizando requisicao para a url e salvando como string
                jsonData = client.DownloadString(queryUri);  
            }
            
            return jsonData;
        }
        // define a url com a api key
        private static void SetQueryUrl()
        {
            // carrega json de configuração da string
            string programDiretory = Directory.GetCurrentDirectory();
            JsonElement apiConfigJson = JsonDocument.Parse(File.ReadAllText(programDiretory + "\\apiconfig.json")).RootElement;

            queryUrl = apiConfigJson.GetProperty("url").GetString().Replace("{apiKey}",
                apiConfigJson.GetProperty("apiKey").GetString());
        }
    }
}
