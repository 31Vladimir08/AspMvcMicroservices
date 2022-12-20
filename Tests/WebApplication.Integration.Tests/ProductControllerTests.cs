using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.AspNetCore.TestHost;

using NUnit.Framework;

namespace WebApplication.Integration.Tests
{
    public class ProductControllerTests : BaseTest
    {
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = Host.GetTestServer().CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
        }

        [Test]
        [TestCase("Product/GetProducts")]
        [TestCase("Product/CreateProduct")]
        [TestCase("Product/EditProduct/1")]
        public async Task AllGetRoutesAreAccessible(string route)
        {
            // Act
            var response = await _httpClient.GetAsync(route);

            // Assert
            response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
        }

        [Test]
        [TestCase("")]
        public async Task AllPostRoutesAreAccessible(string route)
        {
            // Arrange
            var content = new StringContent(string.Empty);

            // Act
            var response = await _httpClient.PostAsync(route, content);

            // Assert
            response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
        }
    }
}