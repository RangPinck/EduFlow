using EduFlowApi.DTOs.BlockDTOs;
using EduFlowApi.DTOs.CourseDTOs;
using EduFlowApi.DTOs.MaterialDTOs;
using EduFlowApi.DTOs.TaskDTOs;
using EduFlowApi.Interfaces;
using EduFlowApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EduFlowApi.Repositories
{
    public class BlockRepository : IBlockRepository
    {
        private readonly EduFlowDbContext _context;

        public BlockRepository(EduFlowDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveChangesAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }

        public async Task<IEnumerable<ShortBlockDTO>> GetBlocksOfCourseAsync(Guid courseId)
        {
            return await _context.CoursesBlocks.AsNoTracking().Where(x => x.Course == courseId).Select(x => new ShortBlockDTO()
            {
                BlockId = x.BlockId,
                BlockName = x.BlockName,
                BlockDateCreated = x.BlockDateCreated,
                Description = x.Description,
                BlockNumberOfCourse = x.BlockNumberOfCourse
            }).ToListAsync();
        }

        public async Task<bool> AddBlockAsync(AddBlockDTO newBlock)
        {
            var course = await _context.CoursesBlocks.Where(x => x.Course == newBlock.Course).OrderByDescending(x => x.BlockNumberOfCourse).FirstOrDefaultAsync();

            int blockNumber = 0;

            if (course != null)
            {
                blockNumber = (int)course.BlockNumberOfCourse;
            }

            CoursesBlock block = new CoursesBlock()
            {
                BlockName = newBlock.BlockName,
                BlockDateCreated = DateTime.UtcNow,
                Course = newBlock.Course,
                Description = newBlock.Description,
                BlockNumberOfCourse = blockNumber + 1
            };

            await _context.CoursesBlocks.AddAsync(block);

            return await SaveChangesAsync();
        }

        public async Task<bool> BlockIsExistByTitleAsync(Guid courseId, string title)
        {
            return await _context.CoursesBlocks.AnyAsync(x => x.BlockName == title && x.Course == courseId);
        }

        public async Task<bool> BlockIsExistByIdAsync(Guid blockId)
        {
            return await _context.CoursesBlocks.AnyAsync(x => x.BlockId == blockId);
        }

        public async Task<StandardCourseDTO> GetCourseByBlockIdAsync(Guid blockId)
        {
            var block = await _context.CoursesBlocks.AsNoTracking().FirstOrDefaultAsync(x => x.BlockId == blockId);

            return await _context.Courses.AsNoTracking().Select(x => new StandardCourseDTO()
            {
                CourseId = x.CourseId,
                CourseName = x.CourseName,
                CourseDataCreate = x.CourseDataCreate,
                Description = x.Description,
                Link = x.Link,
                Author = x.Author
            }).FirstOrDefaultAsync(x => x.CourseId == block.Course);
        }

        public async Task<bool> UpdateBlockAsync(UpdateBlockDTO updateBlock)
        {
            var block = await _context.CoursesBlocks.FirstOrDefaultAsync(x => x.BlockId == updateBlock.BlockId);

            block.BlockName = updateBlock.BlockName;
            block.Description = updateBlock.Description;

            _context.CoursesBlocks.Update(block);

            return await SaveChangesAsync();
        }

        public async Task<bool> CompletedTasksFromTheBlockIsExistsAsync(Guid blockId)
        {
            var listTasksBlock = await _context.BlocksTasks.AsNoTracking().Where(x => x.Block == blockId).Select(x => x.TaskId).ToListAsync();

            if (listTasksBlock.Count == 0)
            {
                return false;
            }

            return await _context.UsersTasks.AnyAsync(x => listTasksBlock.Contains((Guid)x.Task));
        }

        public async Task<bool> DeleteBlockAsync(Guid blockId)
        {
            var block = await _context.CoursesBlocks.FirstOrDefaultAsync(x => x.BlockId == blockId);
            _context.CoursesBlocks.Remove(block);
            return await SaveChangesAsync();
        }

        public async Task<BlockStatisticsDTO> GetBlockStatisticById(Guid blockId, Guid userId)
        {
            var blockData = await _context.CoursesBlocks.AsNoTracking().Include(x => x.BlocksTasks).ThenInclude(x => x.TasksPractices).Include(x => x.BlocksMaterials).FirstAsync(x => x.BlockId == blockId);

            return new BlockStatisticsDTO()
            {
                BlockId = blockData.BlockId,

                BlockName = blockData.BlockName,

                FullyCountTask = blockData.BlocksTasks.Count() + blockData.BlocksMaterials.Where(mat => mat.Duration != null).Count() + blockData.BlocksTasks.Select(x => x.TasksPractices.Count()).Sum(),

                FullyDurationNeeded = blockData.BlocksTasks.Select(x => x.Duration).Sum() + blockData.BlocksMaterials.Where(mat => mat.Duration != null).Select(x => Convert.ToInt32(x.Duration)).Sum() + blockData.BlocksTasks.Select(x => x.TasksPractices.Select(x => Convert.ToInt32(x.Duration)).Sum()).Sum(),

                CompletedTaskCount = _context.UsersTasks.Where(x => x.AuthUser == userId && x.Status == 3 && (x.MaterialNavigation.Block == blockData.BlockId || x.TaskNavigation.Block == blockData.BlockId || x.PracticeNavigation.TaskNavigation.Block == blockData.BlockId)).Count(),

                DurationCompletedTask = _context.UsersTasks.Where(x => x.AuthUser == userId && x.Status == 3 && (x.MaterialNavigation.Block == blockData.BlockId || x.TaskNavigation.Block == blockData.BlockId || x.PracticeNavigation.TaskNavigation.Block == blockData.BlockId)).Sum(x => x.DurationMaterial + x.DurationPractice + x.DurationTask),

                PercentCompletedTask = Math.Round((double)_context.UsersTasks.Where(x => x.AuthUser == userId && x.Status == 3 && (x.MaterialNavigation.Block == blockData.BlockId || x.TaskNavigation.Block == blockData.BlockId || x.PracticeNavigation.TaskNavigation.Block == blockData.BlockId)).Count() / (double)(blockData.BlocksTasks.Count() + blockData.BlocksMaterials.Where(mat => mat.Duration != null).Count() + blockData.BlocksTasks.Select(x => x.TasksPractices.Count()).Sum()) * 100.0, 2),

                PercentDurationCompletedTask = Math.Round((double)_context.UsersTasks.Where(x => x.AuthUser == userId && x.Status == 3 && (x.MaterialNavigation.Block == blockData.BlockId || x.TaskNavigation.Block == blockData.BlockId || x.PracticeNavigation.TaskNavigation.Block == blockData.BlockId)).Sum(x => x.DurationMaterial + x.DurationPractice + x.DurationTask) / (double)(blockData.BlocksTasks.Select(x => x.Duration).Sum() + blockData.BlocksMaterials.Where(mat => mat.Duration != null).Select(x => Convert.ToInt32(x.Duration)).Sum() + blockData.BlocksTasks.Select(x => x.TasksPractices.Select(x => Convert.ToInt32(x.Duration)).Sum()).Sum()) * 100.0, 2),
            };
        }

        public async Task<List<FullBlockDTO>> GetFullBlocksData(Guid courseId, Guid userId)
        {
            var blocks = await _context.CoursesBlocks.AsNoTracking().Where(x => x.Course == courseId).ToListAsync();

            List<Guid> blocksIds = blocks.Select(x => x.BlockId).ToList();

            var taskRepository = new TaskRepository(_context);
            var materialRepository = new MaterialRepository(_context);

            List<(Guid blockId, IEnumerable<TaskDTO> tasks, int countTasks)> tasksByBlocksIds = new List<(Guid blockId, IEnumerable<TaskDTO> tasks, int countTasks)>();

            List<(Guid blockId, IEnumerable<MaterialDTO> materials)> materialsByBlocksIds = new List<(Guid blockId, IEnumerable<MaterialDTO> materials)>();

            List<(Guid blockId, BlockStatisticsDTO blockStats)> blockStatisctics = new List<(Guid blockId, BlockStatisticsDTO blockStats)>();

            foreach (var item in blocksIds)
            {
                var tasks = await taskRepository.GetTasksOfBlockIdAsync(item, userId);
                tasksByBlocksIds.Add((item, tasks, tasks.Count()));
                materialsByBlocksIds.Add((item, (await materialRepository.GetMaterialsByBlocksIds(item, userId))));
                blockStatisctics.Add((item, (await GetBlockStatisticById(item, userId))));
            }

            return blocks.Select(block => new FullBlockDTO()
            {
                BlockId = block.BlockId,
                BlockName = block.BlockName,
                BlockDateCreated = block.BlockDateCreated,
                Description = block.Description,
                BlockNumberOfCourse = block.BlockNumberOfCourse,
                Tasks = tasksByBlocksIds.First(x => x.blockId == block.BlockId).tasks,
                FullyCountTask = tasksByBlocksIds.First(x => x.blockId == block.BlockId).countTasks,
                Materials = materialsByBlocksIds.First(x => x.blockId == block.BlockId).materials,

                FullyDurationNeeded = blockStatisctics.First(x => x.blockId == block.BlockId).blockStats.FullyDurationNeeded,
                CompletedTaskCount = blockStatisctics.First(x => x.blockId == block.BlockId).blockStats.CompletedTaskCount,
                DurationCompletedTask = blockStatisctics.First(x => x.blockId == block.BlockId).blockStats.DurationCompletedTask,
                PercentCompletedTask = blockStatisctics.First(x => x.blockId == block.BlockId).blockStats.PercentCompletedTask,
                PercentDurationCompletedTask = blockStatisctics.First(x => x.blockId == block.BlockId).blockStats.PercentDurationCompletedTask,
            }).OrderBy(x => x.BlockNumberOfCourse).ToList();
        }
    }
}
