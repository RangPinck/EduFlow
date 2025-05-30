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

                FileContentResult file;

                string tempFile = Path.GetTempFileName();

                using (var stream = new FileStream(
                         filepath,
                         FileMode.Open,
                         FileAccess.Read,
                         FileShare.ReadWrite)) // Важно!
                {
                    try
                    {
                        System.IO.File.Copy(filepath, tempFile, overwrite: true);
                        var bytes = System.IO.File.ReadAllBytes(tempFile);
                        file = new FileContentResult(bytes, contentType)
                        {
                            FileDownloadName = $"EduFlowApiLog-{date.ToString()}.txt"
                        };
                    }
                    finally
                    {
                        System.IO.File.Delete(tempFile);
                    }
                }


                //using (var stream = new FileStream(
                //         filepath,
                //         FileMode.Open,
                //         FileAccess.Read,
                //         FileShare.ReadWrite)) // Важно!
                //{
                //    var bytes = await System.IO.File.ReadAllBytesAsync(filepath);

                //    file = new FileContentResult(bytes, contentType)
                //    {
                //        FileDownloadName = $"EduFlowApiLog-{date.ToString()}.txt"
                //    };
                //}

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return file;
            }
            catch (FileNotFoundException file)
            {
                return BadRequest("File not found!");
            }
            catch (Exception ex)
            {
                Log.Error($"get logs => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }
    }
}
