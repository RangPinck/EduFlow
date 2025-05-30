using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Serilog;
using EduFlowApi.DTOs.UserDTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace EduFlowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        [SwaggerOperation(Summary = "Получение логов")]
        [HttpGet("GetLogs")]
        [ProducesResponseType(200, Type = typeof(FileContentResult))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> GetLogs(DateOnly date)
        {
            try
            {
                if (date == DateOnly.MinValue)
                {
                    date = DateOnly.FromDateTime(DateTime.Now);
                }

                if (date > DateOnly.FromDateTime(DateTime.Now))
                {
                    return BadRequest("This date has not arrived yet!");
                }

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"EduFlowApiLog-{date.ToString("yyyyMMdd")}.txt");

                var provider = new FileExtensionContentTypeProvider();

                if (!provider.TryGetContentType(filepath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                var bytes = await System.IO.File.ReadAllBytesAsync(filepath);

                var file = new FileContentResult(bytes, contentType)
                {
                    FileDownloadName = $"EduFlowApiLog-{date.ToString()}.txt"
                };

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return file;
            }
            catch (Exception ex)
            {
                Log.Error($"get logs => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }
    }
}
