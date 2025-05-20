using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduFlowApi.DTOs.CheckConnectionDTOs;
using EduFlowApi.Models;

namespace EduFlowApi.Interfaces
{
    public interface IConnectionRepository
    {
        public Task<CheckConnectionDTO> CheckConnectionAsyncAsync();

        public EduFlowDbContext CreateNewContext();
    }
}
