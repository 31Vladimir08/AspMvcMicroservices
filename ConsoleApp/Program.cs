using System;
using System.Collections.Generic;
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
            Parallel.ForEach(
                products,
                x =>
                {
                    Console.WriteLine($"ProductId: {x.ProductID}\tName: {x.ProductName}\tCategoryID: " +
                                      $"{x.CategoryID}\tCategory: {x.Сategory}");
                });
        }

        static void ShowCategories(IEnumerable<Сategory> products)
        {
            foreach (var item in products)
            {
                Console.WriteLine($"CategoryID: {item.CategoryID}\tName: {item.CategoryName}");
            }
        }

        static async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var response = await client.GetAsync($"api/ProductApi");
            var products = response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<IList<Product>>() : new List<Product>();
            return products;
        }

        static async Task<IEnumerable<Сategory>> GetCategoriesAsync()
        {
            var response = await client.GetAsync($"api/CategoryApi");
            var products = response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<IList<Сategory>>() : new List<Сategory>();
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
                var product = GetProductsAsync();
                var categories = GetCategoriesAsync();
                Task.WaitAll(product, categories);
                ShowProducts(product.Result);
                ShowCategories(categories.Result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
