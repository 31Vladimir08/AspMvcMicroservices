using NUnit.Framework;
using WebApplication.NSwag.MyNamespace;

namespace WebApplication.NSwag
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async void Test1()
        {
            var client = new ProductApiClient(new System.Net.Http.HttpClient());
            var products = await client.GetProductsAsync();
            Assert.Pass();
        }
    }
}