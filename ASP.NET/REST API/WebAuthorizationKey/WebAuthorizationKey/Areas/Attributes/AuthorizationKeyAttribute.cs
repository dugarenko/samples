using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;
using WebAuthorizationKey.Properties;

namespace WebAuthorizationKey.Areas.Attributes
{
    public class AuthorizationKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string API_KEY_NAME = "ApiKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    // Nie podano danych uwierzytelniających.
                    Content = Resources.Content_StatusCode_401_NiePodanoDanychUwierzytelniajacych
                };
                return;
            }

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(API_KEY_NAME);

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    // Niepoprawny token.
                    Content = Resources.Content_StatusCode_401_NiepoprawnyToken
                };
                return;
            }

            await next();
        }
    }
}
