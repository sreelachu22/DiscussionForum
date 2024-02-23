using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace DiscussionForum.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] _requiredRoles;

        public CustomAuthAttribute(string policyName)
        {
            if (policyName == "Admin")
            {
                _requiredRoles = new string[] { "SuperAdmin" };
            }
            else if (policyName == "Head")
            {
                _requiredRoles = new string[] { "SuperAdmin", "CommunityHead" };
            }
            else if (policyName == "User")
            {
                _requiredRoles = new string[] { "SuperAdmin", "CommunityHead", "User" };
            }
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            bool isInAnyRole = false;
            foreach (var role in _requiredRoles)
            {
                if (context.HttpContext.User.IsInRole(role))
                {
                    isInAnyRole = true;
                    break;
                }
            }

            if (!isInAnyRole)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                return;
            }
        }
    }
}
