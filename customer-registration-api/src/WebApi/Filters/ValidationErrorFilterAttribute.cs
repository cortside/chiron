using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Chiron.Registration.Customer.WebApi.Filters {
    public class ValidationErrorFilterAttribute : IAsyncExceptionFilter {
        readonly ILogger<ValidationErrorFilterAttribute> logger;
        public ValidationErrorFilterAttribute(ILogger<ValidationErrorFilterAttribute> logger) {
            this.logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context) {
            var ex = context.Exception;
            if (ex is ValidationException) {
                var body = JsonConvert.SerializeObject(new {
                    Message = ex.Message
                });
                context.HttpContext.Response.StatusCode = 400;
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(body);
                context.ExceptionHandled = true;
            } else {
                logger.LogError(500, ex, ex.Message);
                context.ExceptionHandled = false;
            }
        }
    }
}
