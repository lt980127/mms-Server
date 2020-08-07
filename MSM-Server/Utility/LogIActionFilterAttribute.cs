using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MSM_Server.Utility
{
    public class LogIActionFilterAttribute:Attribute,IActionFilter
    {
        private readonly ILogger<LogIActionFilterAttribute> _logger = null;

        public LogIActionFilterAttribute(ILogger<LogIActionFilterAttribute> logger)
        {
            this._logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result;
            ObjectResult objectResult = result as ObjectResult;
            var actionlog =
                $"{DateTime.Now} 调用 {context.RouteData.Values["action"]} api完成;执行结果：{Newtonsoft.Json.JsonConvert.SerializeObject(objectResult != null ? objectResult.Value : "")}";
            _logger.LogInformation(actionlog);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var resetlogs = $"{ DateTime.Now} 开始调用 {context.RouteData.Values["action"]} api;参数为：{Newtonsoft.Json.JsonConvert.SerializeObject(context.ActionArguments)}";
            _logger.LogInformation(resetlogs);
        }
    }
}
