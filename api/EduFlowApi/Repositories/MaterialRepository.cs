using Microsoft.EntityFrameworkCore;
using EduFlowApi.DTOs.MaterialDTOs;
using EduFlowApi.Interfaces;
using EduFlowApi.Models;

namespace EduFlowApi.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly EduFlowDbContext _context;

        public MaterialRepository(EduFlowDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MaterialTypeDTO>> GetMaterialsTypesAsync()
        {
            return await _context.MaterialTypes.AsNoTracking().Select(material => new MaterialTypeDTO()
            {
                TypeId = material.TypeId,
                TypeName = material.TypeName
            }).ToListAsync();
        }

        public async Task<IEnumerable<MaterialDTO>> GetMaterialsAsync(Guid userId, Guid blockId, int materialTypeId)
        {
            var studyRepository = new StatusStudyRepository(_context);

            var materials = await _context.BlocksMaterials.AsNoTracking().Where(material => material.Block == blockId && material.MaterialNavigation.Type == materialTypeId).ToListAsync();

            List<MaterialDTO> result = new List<MaterialDTO>();

            foreach (var item in materials)
            {
                result.Add(
                    new MaterialDTO()
                    {
                        MaterialId = item.MaterialNavigation.MaterialId,
                        BlockMaterialId = item.BmId,
                        MaterialName = item.MaterialNavigation.MaterialName,
                        MaterialDateCreate = item.MaterialNavigation.MaterialDateCreate,
                        Link = item.MaterialNavigation.Link,
                        TypeId = item.MaterialNavigation.Type,
                        TypeName = item.MaterialNavigation.TypeNavigation.TypeName,
                        Description = item.MaterialNavigation.Description,
                        DurationNeeded = item.Duration,
                        Note = item.Note,
                        BmDateCreate = item.BmDateCreate,
                        Status = await studyRepository.CheckStateByIdAsync(item.BmId, userId),
                        DateStart = await studyRepository.GetDateStart(item.BmId, userId),
                        Duration = await studyRepository.GetDuration(item.BmId, userId),
                    });
            }
            return result;
        }

        public async Task<IEnumerable<MaterialDTO>> GetMaterialsByBlocksIds(Guid blockId, Guid userId)
        {
            var studyRepository = new StatusStudyRepository(_context);

            var materialsBlocks = await _context.BlocksMaterials.AsNoTracking().Where(x => x.Block == blockId).Include(x => x.MaterialNavigation).ThenInclude(x => x.TypeNavigation).ToListAsync();

            List<MaterialDTO> results = new List<MaterialDTO>();

            foreach (var item in materialsBlocks)
            {
                results.Add(
                    new MaterialDTO()
                    {
                        MaterialId = item.MaterialNavigation.MaterialId,
                        BlockMaterialId = item.BmId,
                        MaterialName = item.MaterialNavigation.MaterialName,
                        MaterialDateCreate = item.MaterialNavigation.MaterialDateCreate,
                        Link = item.MaterialNavigation.Link,
                        TypeId = item.MaterialNavigation.Type,
                        TypeName = item.MaterialNavigation.TypeNavigation.TypeName,
                        Description = item.MaterialNavigation.Description,
                        DurationNeeded = item.Duration,
                        Note = item.Note,
                        BmDateCreate = item.BmDateCreate,
                        Status = await studyRepository.CheckStateByIdAsync(item.BmId, userId),
                        DateStart = await studyRepository.GetDateStart(item.BmId, userId),
                        Duration = await studyRepository.GetDuration(item.BmId, userId),
                    }
                );
            }

            return results;
        }

        public async Task<MaterialDTO> GetMaterialByIdAsync(Guid materialId, Guid userId)
        {
            var material = await _context.BlocksMaterials.AsNoTracking().FirstOrDefaultAsync(x => x.BmId == materialId);

            var studyRepository = new StatusStudyRepository(_context);

            return new MaterialDTO()
            {
                MaterialId = material.MaterialNavigation.MaterialId,
                BlockMaterialId = material.BmId,
                MaterialName = material.MaterialNavigation.MaterialName,
                MaterialDateCreate = material.MaterialNavigation.MaterialDateCreate,
                Link = material.MaterialNavigation.Link,
                TypeId = material.MaterialNavigation.Type,
                TypeName = material.MaterialNavigation.TypeNavigation.TypeName,
                Description = material.MaterialNavigation.Description,
                DurationNeeded = material.Duration,
                Note = material.Note,
                BmDateCreate = material.BmDateCreate,
                Status = await studyRepository.CheckStateByIdAsync(materialId, userId),
                DateStart = await studyRepository.GetDateStart(materialId, userId),
                Duration = await studyRepository.GetDuration(materialId, userId),
            };
        }

        public async Task<bool> AddMaterialAsync(AddMaterialDTO newMaterial)
        {
            var material = new Material()
            {
                MaterialName = newMaterial.MaterialName,
                MaterialDateCreate = DateTime.UtcNow,
                Link = newMaterial.Link,
                Type = newMaterial.TypeId,
                Description = newMaterial.Description,
                BlocksMaterials = new List<BlocksMaterial>(){ new BlocksMaterial()
                {
                    Duration = newMaterial.Duration,
                    Block = newMaterial.Block,
                    Note = newMaterial.Note
                }}
            };

            _context.Materials.Attach(material);
            await _context.Materials.AddAsync(material);

            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateMaterialAsync(UpdateMaterialDTO updateMaterial)
        {
            var material = await _context.BlocksMaterials.Include(x => x.MaterialNavigation).FirstOrDefaultAsync(material => material.MaterialNavigation.MaterialId == updateMaterial.MaterialId);

            material.MaterialNavigation.MaterialName = updateMaterial.MaterialName;
            material.MaterialNavigation.Link = updateMaterial.Link;
            material.MaterialNavigation.Type = updateMaterial.TypeId;
            material.MaterialNavigation.Description = updateMaterial.Description;
            material.Duration = updateMaterial.Duration;
            material.Note = updateMaterial.Note;

            _context.BlocksMaterials.Attach(material);
            _context.BlocksMaterials.Update(material);

            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteMaterialAsync(Guid materialId)
        {
            var material = await _context.Materials.FirstOrDefaultAsync(x => x.MaterialId == materialId);

            _context.Materials.Remove(material);

            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }

        public async Task<bool> MaterialComparisonByTitleAndBlockAsync(string title, Guid blockId)
        {
            return await _context.BlocksMaterials.AsNoTracking().AnyAsync(x => x.MaterialNavigation.MaterialName == title && x.Block == blockId);
        }

        public async Task<Guid?> GetAuthorOfCourseByBlocklIdAsync(Guid blockId)
        {
            var block = await _context.CoursesBlocks.AsNoTracking().Include(x => x.CourseNavigation).FirstOrDefaultAsync(x => x.BlockId == blockId);

            return block != null ? block.CourseNavigation.Author : null;
        }

        public async Task<bool> MaterialTypeComparisonByIdAsync(int materialTypeId)
        {
            return await _context.MaterialTypes.AsNoTracking().AnyAsync(x => x.TypeId == materialTypeId);
        }

        public async Task<MaterialShortDTO> GetMaterialDataByIdAsync(Guid materialId)
        {
            return await _context.BlocksMaterials.AsNoTracking().Select(material => new MaterialShortDTO()
            {
                MaterialId = material.MaterialNavigation.MaterialId,
                Author = material.BlockNavigation.CourseNavigation.Author,
            }).FirstOrDefaultAsync(x => x.MaterialId == materialId);
        }

        public async Task<bool> AddMaterialToBlockAsync(MaterialToBlockDTO mb)
        {
            var newBm = new BlocksMaterial()
            {
                Block = mb.Block,
                Material = mb.Material,
                BmDateCreate = DateTime.UtcNow,
                Note = mb.Note,
                Duration = mb.Duration
            };

            await _context.BlocksMaterials.AddAsync(newBm);

            return await SaveChangesAsync();
        }

        public async Task<bool> CheckExistsMaterailOfBlocAsync(Guid blockId, Guid materialId)
        {
            return await _context.BlocksMaterials.AnyAsync(x => x.Block == blockId && materialId == x.Material);
        }

        public async Task<bool> UpdateMaterialToBlockAsync(MaterialToBlockDTO mb)
        {
            var bm = await _context.BlocksMaterials.FirstOrDefaultAsync(x => x.Block == mb.Block && mb.Material == x.Material);

            bm.Duration = mb.Duration;
            bm.Note = mb.Note;

            _context.BlocksMaterials.Update(bm);

            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteMaterialToBlockAsync(Guid blockId, Guid materialId)
        {
            var bm = await _context.BlocksMaterials.FirstOrDefaultAsync(x => x.Block == blockId && materialId == x.Material);

            _context.BlocksMaterials.Remove(bm);

            return await SaveChangesAsync();
        }
    }
}
