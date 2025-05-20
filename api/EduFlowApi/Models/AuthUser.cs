using Microsoft.AspNetCore.Identity;

namespace EduFlowApi.Models
{
    public partial class AuthUser : IdentityUser<Guid>
    {
        public User UserNavigation { get; set; }

        public DateTime UserDataCreate { get; set; }

        public AuthUser() { }

        public AuthUser(string userName) : this()
        {
            UserName = userName;
        }
    }
}
