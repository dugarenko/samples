using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PullPosNumber.Properties;
using System;
using System.Linq;

namespace PullPosNumber.Filters
{
    public class TokenAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private string _token = "FBF10B16EB1D4CA4BC48EDFCFACEDAEA";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization") ||
                context.HttpContext.Request.Headers.First(x => x.Key == "Authorization").Value != _token)
            {
                context.Result = new UnauthorizedObjectResult(Resources.Error_401_Html);
            }
        }
    }
}
