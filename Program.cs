using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using MyDataClass;
using System.Linq;

namespace SuppliesPriceLister
{
    class Program
    {
        public static IConfiguration _config;
        static void Main(string[] args)
        {
            _config = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .Build();

            List<ActualResult> results = new List<ActualResult>();

            ReadFromCSVFile(results);
            ReadFromJSONFile(results);
            OrderByPriceDesc(results);
        }

        private static void ReadFromCSVFile(List<ActualResult> actualResults)
        {
            string file = "..\\..\\..\\humphries.csv";

            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');
                        if (values[0] != "identifier")
                        {
                            actualResults.Add(new ActualResult { Id = values[0], Name = values[1], Price = Math.Round(Double.Parse(values[3]), 2) });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        private static void ReadFromJSONFile(List<ActualResult> actualResults)
        {
            string file = "..\\..\\..\\megacorp.json";

            try
            {
                string jsonText = File.ReadAllText(file);
                WrapperClass data = JsonSerializer.Deserialize<WrapperClass>(jsonText, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                var supplies = data.Partners.Select(x => x.Supplies).ToList();

                List<Supply> suppliesArray = new List<Supply>();
                foreach (var supplier in supplies)
                {
                    suppliesArray.AddRange(supplier);
                }

                suppliesArray.ForEach(x =>
                {
                    actualResults.Add(new ActualResult
                    {
                        Id = x.Id.ToString(),
                        Name = x.Description,
                        Price = Math.Round((x.PriceInCents * 0.01) / Double.Parse(_config["audUsdExchangeRate"]), 2),
                    });
                });
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        private static void OrderByPriceDesc(List<ActualResult> actualResults)
        {
            try
            {
                var sortedListBasedonPrice = actualResults.OrderByDescending(x => x.Price).ToList();
                actualResults = sortedListBasedonPrice;

                Display(actualResults);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        private static void Display(List<ActualResult> actualResults)
        {
            try
            {
                foreach (var item in actualResults)
                {
                    Console.WriteLine($"Id: {item.Id}, Description: {item.Name}, Price: ${item.Price}");
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}
