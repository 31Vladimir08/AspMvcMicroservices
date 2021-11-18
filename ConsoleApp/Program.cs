using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ConsoleApp.Models;

namespace ConsoleApp
{
    class Program
    {
        static HttpClient client = new();

        static void ShowProducts(IEnumerable<Product> products)
        {
            foreach (var item in products)
            {
                Console.WriteLine($"ProductId: {item.ProductID}\tName: {item.ProductName}\tCategoryID: " +
                                  $"{item.CategoryID}\tCategory: {item.Сategory}");
            }
        }
        
        static async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var response = await client.GetAsync($"api/ProductApi");
            var products = response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<IList<Product>>() : new List<Product>();
            return products;
        }
        
        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri(Settings.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Get the product
                var product = await GetProductsAsync();
                ShowProducts(product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
