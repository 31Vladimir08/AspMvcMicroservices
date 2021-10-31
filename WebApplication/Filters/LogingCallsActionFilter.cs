using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebApplication.Filters
{
    public class LogingCallsActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<LogingCallsActionFilter> _loger;

        public LogingCallsActionFilter(ILogger<LogingCallsActionFilter> loger)
        {
            _loger = loger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var d = context.ModelState;
            _loger.LogInformation($"{context.ActionDescriptor.DisplayName}: START");
            await next();
            _loger.LogInformation($"{context.ActionDescriptor.DisplayName}: FINISH");
        }
    }
}
