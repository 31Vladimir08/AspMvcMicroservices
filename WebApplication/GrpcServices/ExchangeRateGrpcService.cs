using System.Threading.Tasks;

using ExchangeRatesAgainstDollar.Grpc.Protos;

using Microsoft.Extensions.Logging;

using WebApplication.Controllers;
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

            if (string.IsNullOrWhiteSpace(response.Error))
            {
                _logger.LogError(response.Error);
            }

            var value = ConvertExchangeRateModelToDecimal(response);

            return new Currensy() 
            {
                    CurrnesyCode = currensyCode,
                    Value = value
            };
        }

        private decimal ConvertExchangeRateModelToDecimal(ExchangeRateModel model)
        {
            var result = System.Convert.ToDecimal($"{model.Nanos}.{model.Units}");
            return result;
        }
    }
}
