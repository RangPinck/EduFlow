using System;
using System.Collections.Generic;
namespace EduFlowApi.DTOs.RoleDTOs
{
    public class RoleDTO
    {
        public Guid RoleId { get; set; }

        public string RoleName { get; set; } = null!;
    }
}
