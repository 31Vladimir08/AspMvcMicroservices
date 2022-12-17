using System.Collections.Generic;
using System.Threading.Tasks;

using ExchangeRatesAgainstDollar.Grpc.Protos;

using Microsoft.Extensions.Logging;

using WebApplication.Models.Settings;

namespace WebApplication.GrpcServices
{
    public class ExchangeRateGrpcService
    {
        private readonly ExchangeRateService.ExchangeRateServiceClient _exchangeRateServiceClient;
        private readonly ILogger<ExchangeRateGrpcService> _logger;

        public ExchangeRateGrpcService(
            ExchangeRateService.ExchangeRateServiceClient exchangeRateServiceClient,
            ILogger<ExchangeRateGrpcService> logger)
        {
            _exchangeRateServiceClient = exchangeRateServiceClient;
            _logger = logger;
        }

        public async Task<Currensy> GetTheExchangeRateAsync(string currensyCode)
        {
            var request = new GetTheExchangeRateRequest()
            {
                CurrencyCode = currensyCode
            };

            var response = await _exchangeRateServiceClient.GetTheExchangeRateAsync(request);

            if (!string.IsNullOrWhiteSpace(response.Error))
            {
                _logger.LogError(response.Error);
                return null;
            }

            var value = ConvertExchangeRateModelToDecimal(response);

            return new Currensy() 
            {
                    CurrnesyCode = currensyCode,
                    Value = value
            };
        }

        public async Task<IEnumerable<Currensy>> GetTheExchangeRateListAsync()
        {
            var response = await _exchangeRateServiceClient.GetTheExchangeRateListAsync(new GetTheExchangeRateAllRequest());

            var result = new List<Currensy>();
            if (!string.IsNullOrWhiteSpace(response.Error))
            {
                _logger.LogError(response.Error);
                return result;
            }

            foreach (var item in response.ExchangesRates)
            {
                var value = ConvertExchangeRateModelToDecimal(item);
                result.Add(new Currensy()
                {
                    CurrnesyCode = item.CurrencyCode,
                    Value = value
                });
            }

            return result;
        }

        private decimal ConvertExchangeRateModelToDecimal(ExchangeRateModel model)
        {
            decimal.TryParse($"{model.Nanos}.{model.Units}", out var result);
            return result;
        }
    }
}
