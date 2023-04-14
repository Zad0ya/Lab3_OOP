using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using lab3_2;

namespace Lab3_2
{
    internal class Program
    {
        Product myObject = new Product();
        static void Main(string[] args)
        {
            Random random = new Random();

            for (int i = 1; i <= 5; i++)
            {
                List<Product> products = new List<Product>();

                for (int j = 1; j <= 10; j++)
                {
                    Product product = new Product
                    {
                        Product_Name = $"Product {j}",
                        Product_Category = $"Category {random.Next(1, 4)}",
                        Product_Price = random.NextDouble() * 100
                    };

                    products.Add(product);
                }

                string json = JsonSerializer.Serialize(products);
                string filename = $"jsons\\{i}.json";

                using (StreamWriter sw = File.CreateText(filename))
                {
                    sw.Write(json);
                }
            }

            string directoryPath = @"jsons\";
            string category = "Category 3";
            double Price_min = 6.0;
            double Price_max = 66.0;

            Predicate<Product> filter = p => p.Product_Category == category && p.Product_Price >= Price_min && p.Product_Price <= Price_max;
            Action<Product> display = p => Console.WriteLine($"Назва продукту: {p.Product_Name}, Категорія: {p.Product_Category}, Ціна: {p.Product_Price}");

            for (int i = 1; i <= 5; i++)
            {
                string filePath = Path.Combine(directoryPath, $"{i}.json");
                ReadList(filePath, filter, display);
            }
        }

        static void ReadList(string filep, Predicate<Product> filter, Action<Product> display)
        {
            using (StreamReader reader = new StreamReader(filep))
            {
                string json = reader.ReadToEnd();
                Product[] products = JsonSerializer.Deserialize<Product[]>(json);
                products.Where(new Func<Product, bool>(filter)).ToList().ForEach(display);
            }
        }
    }
}
