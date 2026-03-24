using System.Security.Claims;
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
            var nationality = user.FindFirst(ctx => ctx.Type == "Nationality")?.Value;
            var dateOfBirthString = user.FindFirst(ctx => ctx.Type == "DateOfBirth")?.Value;
            var dateOfBirth = dateOfBirthString == null 
                ? (DateOnly?)null
                : DateOnly.ParseExact(dateOfBirthString, "yyyy-MM-dd");
            
            return new CurrentUser(userId, email, roles, nationality, dateOfBirth);
        }
    }
}