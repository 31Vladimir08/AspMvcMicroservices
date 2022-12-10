using System.Net.Http;

using ExchangeRatesAgainstTheDollar.Grpc.Models.Settings;
using ExchangeRatesAgainstTheDollar.Grpc.Protos;

using Grpc.Core;

using HtmlAgilityPack;

using Microsoft.Extensions.Options;

namespace ExchangeRatesAgainstTheDollar.Grpc.Services
{
    public class ExchangeRateService : Protos.ExchangeRateService.ExchangeRateServiceBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WebCurrensySettings _webCurrensySettings;

        public ExchangeRateService (IHttpClientFactory httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
            //_webCurrensySettings = webCurrensySettingsOptions.Value;
        }
        public override async Task<ExchangeRateModel> GetTheExchangeRate(GetTheExchangeRateRequest request, ServerCallContext context)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var s = await httpClient.GetStreamAsync(_webCurrensySettings.Url);

                var html = string.Empty;
                using (var myStreamReader = new StreamReader(s))
                {
                    html = await myStreamReader.ReadToEndAsync();
                }

                HtmlDocument htmlSnippet = new HtmlDocument();
                htmlSnippet.LoadHtml(html);
                
                var titleNode = htmlSnippet.DocumentNode.SelectSingleNode(_webCurrensySettings.PatchToCurrensyElements.First().PatchToCurrensyElement);
            }
            return new ExchangeRateModel()
            {
                CurrencyCode = request.CurrencyCode,
                Nanos = 1,
                Units = 2
            };
        }
    }
}
