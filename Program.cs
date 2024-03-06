using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using MyDataClass;

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
    }
}
