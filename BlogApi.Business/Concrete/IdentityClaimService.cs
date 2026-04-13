using System.Security.Claims;
using BlogApi.DataAccess.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Business.Concrete
{
    public class IdentityClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IdentityClaimService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int FindUserId()
        {
            var userIdClaim =  _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new Exception("didnt find user id");
            }

             if (!int.TryParse(userIdClaim, out int userId))
            {
                throw new Exception("User id claim geçerli bir int değil.");
            }
            
            return userId;
        }
    }
}