﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using EduFlowApi.DTOs.BlockDTOs;
using EduFlowApi.DTOs.CourseDTOs;
using EduFlowApi.Interfaces;
using EduFlowApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace EduFlowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly IBlockRepository _blockRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly UserManager<AuthUser> _userManager;

        public BlockController(IBlockRepository blockRepository, ICourseRepository courseRepository, UserManager<AuthUser> userManager)
        {
            _blockRepository = blockRepository;
            _courseRepository = courseRepository;
            _userManager = userManager;
        }

        [SwaggerOperation(Summary = "Получение блоков (разделов) курса")]
        [HttpGet("GetBlockOfCourse")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ShortBlockDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetBlockOfCourse(Guid courseId)
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();
                var subCheck = new SubscribeUserCourseDTO()
                {
                    courseId = courseId,
                    userId = authUser.Id,
                };

                IEnumerable<ShortBlockDTO> blocks = new List<ShortBlockDTO>();

                if (!authUserRoles.Contains("Администратор") && !authUserRoles.Contains("Куратор") && !await _courseRepository.CheckUserSubscribeOnCourseAsync(subCheck))
                {
                    return BadRequest("User doesnt subscribe to course!");
                }
                else
                {
                    blocks = await _blockRepository.GetBlocksOfCourseAsync(courseId);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(blocks);
            }
            catch (Exception ex)
            {
                Log.Error($"get blocks of course => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Добавление блока (раздела) курса")]
        [HttpPost("AddBlock")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> AddBlock(AddBlockDTO newBlock)
        {
            try
            {
                if (!await _courseRepository.CourseIsExistByIdAsync(newBlock.Course))
                {
                    return BadRequest("Course not found!");
                }

                if (string.IsNullOrEmpty(newBlock.BlockName))
                {
                    return BadRequest("A block cannot exist without title.");
                }

                if (await _blockRepository.BlockIsExistByTitleAsync(newBlock.Course, newBlock.BlockName))
                {
                    return BadRequest("This block already exist.");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();
                var course = await _courseRepository.GetCourseDataByIdAsync(newBlock.Course);

                if (!authUserRoles.Contains("Администратор") && authUser.Id != course.Author)
                {
                    return BadRequest("You don't have enough rights for this operation!");
                }

                if (!await _blockRepository.AddBlockAsync(newBlock))
                {
                    return BadRequest("This block doesn't add to database. No correct data.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok("Operation success");
            }
            catch (Exception ex)
            {
                Log.Error($"add block => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }


        [SwaggerOperation(Summary = "Обновление блока")]
        [HttpPut("UpdateBlock")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> UpdateBlock(UpdateBlockDTO updateBlock)
        {
            try
            {
                var course = await _blockRepository.GetCourseByBlockIdAsync(updateBlock.BlockId);
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    if (authUser.Id != course.Author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _blockRepository.BlockIsExistByIdAsync(updateBlock.BlockId))
                {
                    return BadRequest("This block not found.");
                }

                if (await _blockRepository.BlockIsExistByTitleAsync(course.CourseId, updateBlock.BlockName, updateBlock.BlockId))
                {
                    return BadRequest("Block with this title was exists!");
                }

                if (!await _blockRepository.UpdateBlockAsync(updateBlock))
                {
                    return BadRequest("This block doesn't update on database. No correct data.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok("Operation success");
            }
            catch (Exception ex)
            {
                Log.Error($"update block => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Удаление блока")]
        [HttpDelete("DeleteBlock")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> DeleteBlock(Guid blockId)
        {
            try
            {
                var course = await _blockRepository.GetCourseByBlockIdAsync(blockId);
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    if (authUser.Id != course.Author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _blockRepository.BlockIsExistByIdAsync(blockId))
                {
                    return BadRequest("This block not found.");
                }

                if (await _blockRepository.CompletedTasksFromTheBlockIsExistsAsync(blockId))
                {
                    return BadRequest("This block doesn't delete. Users have complete task of this block.");
                }

                if (!await _blockRepository.DeleteBlockAsync(blockId))
                {
                    return BadRequest("This block doesn't delete. No correct data.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok("Operation success");
            }
            catch (Exception ex)
            {
                Log.Error($"delete block => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }
    }
}
