using EduFlowApi.Models;

namespace EduFlowApi.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(AuthUser user, string role);
    }
}
