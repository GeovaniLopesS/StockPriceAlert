using StockPriceAlert;

class Program
{
   public static void Main(string[] args)
    {

        // Aguarda 5 segundos entre cada chama do metodo de envio do email
        Timer timer = new Timer(CheckQuotePrice, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        Console.ReadLine();
    }

    private static void CheckQuotePrice(object state)
    {
        // Simula um valor de cotação.
        Random random = new Random();
        decimal stockQuote = random.Next(5, 25);
        Console.WriteLine(stockQuote);

        // Verifica se a cotação atinge algum limite.
        if (stockQuote <= 10)
        {
            // Envia o email
            EmailSender.SendEmail("stockquotealert1@gmail.com", "geovanilopes2002@gmail.com", "compre!");

        }
        else if (stockQuote >= 20)
        {
            EmailSender.SendEmail("stockquotealert1@gmail.com", "geovanilopes2002@gmail.com", "venda!");
        }
    }
}