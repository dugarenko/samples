using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Shop.Api.Authorization
{
    public class AllowAnonymous : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (var requirement in context.PendingRequirements)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
