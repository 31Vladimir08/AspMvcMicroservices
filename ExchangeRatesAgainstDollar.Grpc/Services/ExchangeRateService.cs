using ExchangeRatesAgainstDollar.Grpc.Models.Settings;
using ExchangeRatesAgainstDollar.Grpc.Protos;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using HtmlAgilityPack;

using Microsoft.Extensions.Options;

namespace ExchangeRatesAgainstDollar.Grpc.Services
{
    public class ExchangeRateService: Protos.ExchangeRateService.ExchangeRateServiceBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WebCurrensySettings _webCurrensySettings;
        private readonly ILogger<ExchangeRateService> _logger;

        public ExchangeRateService(
            IHttpClientFactory httpClientFactory, 
            IOptions<WebCurrensySettings> webCurrensySettingsOptions,
            ILogger<ExchangeRateService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _webCurrensySettings = webCurrensySettingsOptions.Value;
            _logger = logger;
        }

        public override async Task<ExchangeRateModel> GetTheExchangeRate(GetTheExchangeRateRequest request, ServerCallContext context)
        {
            try
            {
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    var sr = await httpClient.GetStreamAsync(_webCurrensySettings.Url);

                    var html = string.Empty;
                    using (var myStreamReader = new StreamReader(sr))
                    {
                        html = await myStreamReader.ReadToEndAsync();
                    }

                    var htmlSnippet = new HtmlDocument();
                    htmlSnippet.LoadHtml(html);

                    var titleNode = htmlSnippet.DocumentNode.SelectSingleNode(
                        _webCurrensySettings.PatchToCurrensyElements.FirstOrDefault(x => x.CurrnesyCode == request.CurrencyCode)?.PatchToCurrensyElement)?.InnerText;
                    var value = ConvertResponseToExchangeRateModel(titleNode);

                    return new ExchangeRateModel()
                    {
                        CurrencyCode = request.CurrencyCode,
                        Error = string.Empty,
                        Nanos = value.nanos,
                        Units = value.units
                    };
                }
            }
            catch (Exception e)
            {
                var er = $"{e.Message}, {e.StackTrace}";
                _logger.LogError(er);
                return new ExchangeRateModel()
                {
                    Error = er
                };
            }            
        }

        private (int nanos, long units) ConvertResponseToExchangeRateModel(string? number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return(0, 0);
            var strArr = number.Split(',', '.');
            if (strArr.Length < 3 ) 
            {
                int n = Convert.ToInt32(strArr[0]);
                int u = strArr.Length == 1 
                    ? Convert.ToInt32(strArr[1])
                    : 0;
                return (n, u);
            }

            return (0, 0);
        }
    }
}
