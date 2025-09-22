using Domain.Interface.HttpContext;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infraestructure.HttpAcessor;

public class ContextUser(IHttpContextAccessor acessor) : IContextUser
{
    private readonly IHttpContextAccessor _contextAccessor = acessor;
    public int Id
    {
        get
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(userIdString) ? 0 : int.Parse(userIdString);
        }
    }
    public string? Email => _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
}
