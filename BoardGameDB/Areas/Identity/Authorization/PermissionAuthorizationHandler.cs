using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BoardGameDB.Areas.Identity.Authorization
{
    public class PermissionAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            if(context.User.IsInRole(Role.Administrator))
            {
                context.Succeed(requirement);
            }
            else if (requirement.Name == OperationName.Create
                    || requirement.Name == OperationName.Edit
                    || requirement.Name == OperationName.Delete)
            {
                if (context.User.IsInRole(Role.Editor))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}