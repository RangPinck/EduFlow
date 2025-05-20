using Microsoft.AspNetCore.Identity;

namespace EduFlowApi.Models
{
    public partial class Role : IdentityRole<Guid>
    {
        public Role() { }

        public bool IsNoManipulate { get; set; } = false;

        public Role(string roleName) : this()
        {
            Name = roleName;
        }
    }
}