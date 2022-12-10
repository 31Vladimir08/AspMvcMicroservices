using WebApplication.Filters;

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using WebApplication.Models;
using WebApplication.GrpcServices;
using System.Threading.Tasks;
using WebApplication.ViewModels;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApplication.Models.Settings;
using Microsoft.Extensions.Configuration;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ExchangeRateGrpcService _exchangeRateGrpcService;
        private readonly ILogger<HomeController> _logger;
        private readonly CurrensyTypes _currensyTypes;
        public HomeController(
            ExchangeRateGrpcService exchangeRateGrpcService, 
            ILogger<HomeController> logger,
            IOptions<CurrensyTypes> currensyTypes,
            IConfiguration configuration) 
        {
            _exchangeRateGrpcService = exchangeRateGrpcService;
            _logger = logger;
            _currensyTypes = currensyTypes.Value;
        }

        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public async Task<IActionResult> Index()
        {
            var vm = new IndexViewModel();
            try
            {
                foreach (var item in _currensyTypes.CurrensyList)
                {
                    var currensy = await _exchangeRateGrpcService.GetTheExchangeRateAsync(item);
                    vm.Currensies.Add(currensy);
                }
            }
            catch (RpcException e)
            {
                _logger.LogError($"{e.Message}");
            }

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
