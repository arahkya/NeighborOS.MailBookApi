using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MailBookApi.Extensions.Authorize
{
    public class DefaultAuthorizationHandler : AuthorizationHandler<DefaultAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DefaultAuthorizationRequirement requirement)
        {
            context.Succeed((IAuthorizationRequirement)requirement);

            return Task.CompletedTask;
        }
    }
}