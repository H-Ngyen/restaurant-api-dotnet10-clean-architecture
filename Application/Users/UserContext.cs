using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Users
{
    public interface IUserContext
    {
        CurrentUser? GetCurrentUser();
    }

    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public CurrentUser? GetCurrentUser()
        {
            var user = httpContextAccessor?.HttpContext?.User;
            if (user == null)
                throw new InvalidOperationException("User context is not present");

            if (user.Identity == null || !user.Identity.IsAuthenticated)
                return null;

            var userId = user.FindFirst(ctx => ctx.Type == ClaimTypes.NameIdentifier)!.Value;
            var email = user.FindFirst(ctx => ctx.Type == ClaimTypes.Email)!.Value;
            var roles = user.Claims.Where(ctx => ctx.Type == ClaimTypes.Role)!.Select(ctx => ctx.Value);

            return new CurrentUser(userId, email, roles);
        }
    }
}