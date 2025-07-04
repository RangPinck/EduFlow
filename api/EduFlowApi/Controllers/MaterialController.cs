﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using EduFlowApi.DTOs.MaterialDTOs;
using EduFlowApi.Interfaces;
using EduFlowApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace EduFlowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly UserManager<AuthUser> _userManager;
        private readonly IBlockRepository _blockRepository;

        public MaterialController(IMaterialRepository materialRepository, UserManager<AuthUser> userManager, IBlockRepository blockRepository)
        {
            _userManager = userManager;
            _blockRepository = blockRepository;
            _materialRepository = materialRepository;
        }

        [SwaggerOperation(Summary = "Получение типов материалов")]
        [HttpGet("GetMaterialsTypes")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MaterialTypeDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> GetMaterialsTypes()
        {
            try
            {
                IEnumerable<MaterialTypeDTO> materialsTypes = await _materialRepository.GetMaterialsTypesAsync();

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(materialsTypes);
            }
            catch (Exception ex)
            {
                Log.Error($"get materials types => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Получение материалов")]
        [HttpGet("GetMaterials")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MaterialDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetMaterials(Guid blockId, int materialTypeId)
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                IEnumerable<MaterialDTO> materials = await _materialRepository.GetMaterialsAsync(authUser.Id, blockId, materialTypeId);

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(materials);
            }
            catch (Exception ex)
            {
                Log.Error($"get all materials => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Получение материала по Id")]
        [HttpGet("GetMaterialById")]
        [ProducesResponseType(200, Type = typeof(MaterialDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetMaterialById(Guid materialId)
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                var material = await _materialRepository.GetMaterialByIdAsync(materialId, authUser.Id);

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(material);
            }
            catch (Exception ex)
            {
                Log.Error($"get material by id => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Добавление материала")]
        [HttpPost("AddMaterial")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> AddMaterial(AddMaterialDTO newMaterial)
        {
            try
            {
                if (await _materialRepository.MaterialComparisonByTitleAndBlockAsync(newMaterial.MaterialName, newMaterial.Block))
                {
                    return BadRequest("This material already exist.");
                }

                if (string.IsNullOrEmpty(newMaterial.MaterialName))
                {
                    return BadRequest("A material cannot exist without title.");
                }

                if (newMaterial.Duration <= 0)
                {
                    return BadRequest("The study time cannot be less than or equal to zero!");
                }

                if (!await _materialRepository.MaterialTypeComparisonByIdAsync(newMaterial.TypeId))
                {
                    return BadRequest("This type of material does not exist!");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    var author = await _materialRepository.GetAuthorOfCourseByBlocklIdAsync(newMaterial.Block);
                    if (author == null || authUser.Id != author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _materialRepository.AddMaterialAsync(newMaterial))
                {
                    return BadRequest("This material doesn't add to database. No correct data.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok("Operation success");
            }
            catch (Exception ex)
            {
                Log.Error($"add material => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Добавление блоку материала")]
        [HttpPost("AddMaterialToBlock")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> AddMaterialToBlock(MaterialToBlockDTO newBm)
        {
            try
            {
                var oldMaterial = await _materialRepository.GetMaterialDataByIdAsync(newBm.Material);

                if (oldMaterial == null)
                {
                    return BadRequest("Material not found!");
                }

                if (!await _blockRepository.BlockIsExistByIdAsync(newBm.Block))
                {
                    return BadRequest("This block not found.");
                }

                if (newBm.Duration <= 0)
                {
                    return BadRequest("The study time cannot be less than or equal to zero!");
                }

                if (await _materialRepository.CheckExistsMaterailOfBlocAsync(newBm.Block, newBm.Material))
                {
                    return BadRequest("The block already has this material!");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    var author = await _materialRepository.GetAuthorOfCourseByBlocklIdAsync(newBm.Block);
                    if (author == null || authUser.Id != author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _materialRepository.AddMaterialToBlockAsync(newBm))
                {
                    return BadRequest("This row doesn't add to database. No correct data.");
                }


                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok("Operation success");
            }
            catch (Exception ex)
            {
                Log.Error($"add material to block => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Обновление материала")]
        [HttpPut("UpdateMaterial")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> UpdateMaterial(UpdateMaterialDTO updatedMaterial)
        {
            try
            {
                var oldMaterial = await _materialRepository.GetMaterialDataByIdAsync(updatedMaterial.MaterialId);

                if (oldMaterial == null)
                {
                    return BadRequest("Material not found!");
                }

                if (string.IsNullOrEmpty(updatedMaterial.MaterialName))
                {
                    return BadRequest("A material cannot exist without title.");
                }

                if (updatedMaterial.Duration <= 0)
                {
                    return BadRequest("The study time cannot be less than or equal to zero!");
                }

                if (!await _materialRepository.MaterialTypeComparisonByIdAsync(updatedMaterial.TypeId))
                {
                    return BadRequest("This type of material does not exist!");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    if (authUser.Id != oldMaterial.Author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _materialRepository.UpdateMaterialAsync(updatedMaterial))
                {
                    return BadRequest("This material doesn't update on database. No correct data.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok("Operation success");
            }
            catch (Exception ex)
            {
                Log.Error($"update materials => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Обновление материала блока")]
        [HttpPut("UpdateMaterialBlock")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> UpdateMaterialBlock(MaterialToBlockDTO bm)
        {
            try
            {
                var oldMaterial = await _materialRepository.GetMaterialDataByIdAsync(bm.Material);

                if (oldMaterial == null)
                {
                    return BadRequest("Material not found!");
                }

                if (!await _blockRepository.BlockIsExistByIdAsync(bm.Block))
                {
                    return BadRequest("This block not found.");
                }

                if (bm.Duration <= 0)
                {
                    return BadRequest("The study time cannot be less than or equal to zero!");
                }

                if (!await _materialRepository.CheckExistsMaterailOfBlocAsync(bm.Block, bm.Material))
                {
                    return BadRequest("The block with this material not found!");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    var author = await _materialRepository.GetAuthorOfCourseByBlocklIdAsync(bm.Block);
                    if (author == null || authUser.Id != author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _materialRepository.UpdateMaterialToBlockAsync(bm))
                {
                    return BadRequest("This row doesn't update on database. No correct data.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok("Operation success");
            }
            catch (Exception ex)
            {
                Log.Error($"update material on block=> {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Удаление материала")]
        [HttpDelete("DeleteMaterial")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> DeleteMaterial(Guid materialId)
        {
            try
            {
                var material = await _materialRepository.GetMaterialDataByIdAsync(materialId);

                if (material == null)
                {
                    return BadRequest("Material not found!");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    if (authUser.Id != material.Author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _materialRepository.DeleteMaterialAsync(materialId))
                {
                    return BadRequest("This material doesn't delete. No correct data.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok("Operation success");
            }
            catch (Exception ex)
            {
                Log.Error($"delete material => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Удаление материала у блока")]
        [HttpDelete("DeleteMaterialOfBlock")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> DeleteMaterialOfBlock(Guid blockId, Guid materialId)
        {
            try
            {
                var oldMaterial = await _materialRepository.GetMaterialDataByIdAsync(materialId);

                if (oldMaterial == null || !await _blockRepository.BlockIsExistByIdAsync(blockId))
                {
                    return BadRequest("Row with this data not found!");
                }

                if (!await _materialRepository.CheckExistsMaterailOfBlocAsync(blockId, materialId))
                {
                    return BadRequest("The block with this material not found!");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    var author = await _materialRepository.GetAuthorOfCourseByBlocklIdAsync(blockId);
                    if (author != null || authUser.Id != author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _materialRepository.DeleteMaterialToBlockAsync(blockId, materialId))
                {
                    return BadRequest("This material doesn't delete. No correct data.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok("Operation success");
            }
            catch (Exception ex)
            {
                Log.Error($"delete material of block => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }
    }
}
