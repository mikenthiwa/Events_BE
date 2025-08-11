using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Helper;

public class AppUser(IHttpContextAccessor contextAccessor) : ClaimsPrincipal(contextAccessor.HttpContext.User)
{
    public string? UserId => FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string? Email => FindFirst(ClaimTypes.Email)?.Value;
}
