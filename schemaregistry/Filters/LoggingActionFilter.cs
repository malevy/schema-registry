using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace SchemaRegistry.Filters
{
    public class LoggingActionFilter : ActionFilterAttribute
    {
        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var descriptor = context.ActionDescriptor.DisplayName;
            _logger.LogDebug($"{descriptor} is being executed");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            var descriptor = context.ActionDescriptor.DisplayName;
            _logger.LogDebug($"{descriptor} completed");
        }
    }
}