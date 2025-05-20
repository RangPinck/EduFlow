using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduFlowApi.DTOs.CheckConnectionDTOs;
using EduFlowApi.Interfaces;
using EduFlowApi.Models;

namespace EduFlowApi.Repositories
{
    public class ConnectionRepository : IConnectionRepository
    {
        private readonly EduFlowDbContext _context;

        public ConnectionRepository(EduFlowDbContext context)
        {
            _context = context;
        }

        public async Task<CheckConnectionDTO> CheckConnectionAsyncAsync()
        {
            using (var connection = new EduFlowDbContext())
            {
                var connect = new CheckConnectionDTO();

                try
                {
                    connect.IsConnect = (ConnectionEnum)Convert.ToInt32(await connection.Database.CanConnectAsync());
                    return connect;
                }
                catch (Exception ex)
                {
                    connect.IsConnect = ConnectionEnum.NoConnectBD;
                    connect.Error = ex.Message;
                    return connect;
                }
            }
        }

        public EduFlowDbContext CreateNewContext()
        {
            return new EduFlowDbContext();
        }
    }
}
