using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduFlowApi.DTOs.CheckConnectionDTOs
{
    public class CheckConnectionDTO
    {
        public ConnectionEnum IsConnect = ConnectionEnum.Connect;
        public string Error = string.Empty;
    }
}
