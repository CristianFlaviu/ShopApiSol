using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShopApi.Database.Data;
using ShopApi.Database.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Repository
{
    public class UserRepo
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccess;

        public UserRepo(DataContext dataContext, IHttpContextAccessor httpContextAccess)
        {
            _dataContext = dataContext;
            _httpContextAccess = httpContextAccess;
        }

        public async Task<BaseUser> GetCurrentUser()
        {
            var httpContext = _httpContextAccess.HttpContext;
            var handler = new JwtSecurityTokenHandler();
            var jwt = httpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);
            var claims = handler.ReadJwtToken(jwt).Claims;
            var email = claims.FirstOrDefault(x => x.Type.Equals("email"))?.Value;

            return (BaseUser)await _dataContext.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));
        }
    }
}
