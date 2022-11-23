using EtherscanAssignment.Infrastructure.Persistence;
using EtherscanAssignment.Infrastructure.Persistence.Entities;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrawlDataConsoleApp
{
    class Response
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
    }

    class Program
    {
        private static async Task<Response> GetPrice(HttpClient client, string symbol)
        {
            decimal price = 0;
            var response = new Response() { Symbol = symbol, Price = price };
            if (string.IsNullOrEmpty(symbol) || client == null) return response;
            string url = $"https://min-api.cryptocompare.com/data/price?fsym={symbol}&tsyms=USD";
            string content = await client.GetStringAsync(url);
            //Console.WriteLine(content);
            if (!string.IsNullOrEmpty(content))
            {
                dynamic dynamicObject = JsonConvert.DeserializeObject<dynamic>(content);
                if (dynamicObject != null && dynamicObject.USD != null)
                {
                    price = (decimal)dynamicObject.USD;
                    response.Price = price;
                }
            }
            return response;
        }

        private static async Task<Response[]> GetPrice(string[] symbols)
        {
            if (symbols == null || symbols.Length == 0) return new Response[0];
            using (var client = new HttpClient())
            {
                var tasks = symbols.Select(symbol => GetPrice(client, symbol));
                var results = await Task.WhenAll(tasks);
                return results;
            }
        }

        private static async Task Process()
        {
            // get symbols
            using (var db = new ApplicationDbContext())
            {
                var tokens = db.Tokens.ToList();
                int count = tokens.Count;

                // to pull token price from API
                string[] symbols = tokens.Select(x => x.Symbol).Distinct().ToArray();
                Response[] responses = await GetPrice(symbols);

                // update to database
                if (responses != null && responses.Length > 0)
                {
                    //var filteredResponses = responses.Where(x => !string.IsNullOrEmpty(x.Symbol));
                    //Dictionary<string, decimal> dictionary = responses.ToDictionary(x => x.Symbol, x => x.Price);
                    bool hasChanged = false;
                    foreach (var response in responses)
                    {
                        //Token foundToken = db.Tokens.FirstOrDefault(x => x.Symbol == response.Symbol);
                        Token foundToken = tokens.FirstOrDefault(x => x.Symbol == response.Symbol);
                        if (foundToken != null)
                        {
                            foundToken.Price = response.Price;
                            hasChanged = true;
                        }
                    }
                    if (hasChanged)
                    {
                        //db.SaveChanges();
                        await db.SaveChangesAsync();
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            int minute = 5;
            while (true)
            {
                Console.WriteLine("Start processing");
                try
                {
                    _ = Process();
                }
                catch (Exception ex)
                {
                    // TODO: add logging.
                    Console.WriteLine(ex);
                }
                Console.WriteLine("Wait for 5 minute");
                System.Threading.Thread.Sleep(minute * 60 * 1000);
            }
        }
    }
}
