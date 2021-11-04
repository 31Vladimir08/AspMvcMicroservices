using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;

namespace WebApplication.Integration.Tests
{
    public class HomeControllerTests : BaseTest
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
        [TestCase("")]
        public async Task AllGetRoutesAreAccessible(string route)
        {
            // Act
            var response = await _httpClient.GetAsync(route);

            // Assert
            response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
        }
    }
}