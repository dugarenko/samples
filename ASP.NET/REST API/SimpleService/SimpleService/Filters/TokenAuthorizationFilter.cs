using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace SimpleService.Filters
{
    public class TokenAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private string _token = "FBF10B16EB1D4CA4BC48EDFCFACEDAEA";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                context.ModelState.AddModelError("Unauthorized", "Authentication credentials were not provided.");
            }
            else if (context.HttpContext.Request.Headers.First(x => x.Key == "Authorization").Value != _token)
            {
                context.ModelState.AddModelError("Unauthorized", "Invalid token.");
            }

            var state = context.ModelState["Unauthorized"];
            if (state != null)
            {
                
                context.Result = new UnauthorizedObjectResult(new { result = "Unauthorized", message = state.Errors.FirstOrDefault()?.ErrorMessage });
                context.HttpContext.Response.Cookies.Append("Cook1", "figa");
            }
        }
    }
}
