using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using EduFlowApi.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace EduFlowApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConnectionController : ControllerBase
    {
        private readonly IConnectionRepository _connectionRepository;

        public ConnectionController(IConnectionRepository connectionRepository)
        {
            _connectionRepository = connectionRepository;
        }

        [SwaggerOperation(Summary = "Проверка подключения к базе данных")]
        [HttpGet("CheckConnection")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public async Task<IActionResult> CheckConnection()
        {
            var connection = await _connectionRepository.CheckConnectionAsyncAsync();

            if (connection.IsConnect == ConnectionEnum.Connect){
                Log.Information($"check connection database => connection success");
                return Ok("Database is connected!");
            }else{
                Log.Error($"check connection database => connection not found");
                return BadRequest(connection.Error);
            }
        }
    }
}
