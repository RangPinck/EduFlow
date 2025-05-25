using EduFlowApi.DTOs.PracticeDTOs;
using EduFlowApi.DTOs.StudyStateDTOs;
using EduFlowApi.DTOs.TaskDTOs;
using EduFlowApi.Interfaces;
using EduFlowApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduFlowApi.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly EduFlowDbContext _context;

        public TaskRepository(EduFlowDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskDTO>> GetTasksOfBlockIdAsync(Guid blockId, Guid userId)
        {
            List<BlocksTask> tasks = await _context.BlocksTasks.AsNoTracking().Where(task => task.Block == blockId).ToListAsync();

            var studyRepository = new StatusStudyRepository(_context);

            List<Guid> ids = tasks.Select(x => x.TaskId).ToList();

            List<(Guid id, StudyStateDTO state)> statuses = await studyRepository.GetStatusesByIdsAsync(ids, userId);

            List<(Guid id, IEnumerable<PracticeDTO> practics)> practics = await new PracticeRepository(_context).GetPracticsByTaskIds(ids, userId);

            List<TaskDTO> result = new List<TaskDTO>();

            foreach (var task in tasks)
            {
                result.Add(new TaskDTO()
                {
                    TaskId = task.TaskId,
                    TaskName = task.TaskName,
                    TaskDateCreated = task.TaskDateCreated,
                    DurationNeeded = task.Duration,
                    Link = task.Link,
                    TaskNumberOfBlock = task.TaskNumberOfBlock,
                    Description = task.Description,

                    Status = statuses.First(x => x.id == task.TaskId).state,

                    DateStart = await studyRepository.GetDateStart(task.TaskId, userId),
                    Duration = await studyRepository.GetDuration(task.TaskId, userId),

                    Practics = practics.First(x => x.id == task.TaskId).practics
                });
            }

            return result.OrderBy(x => x.TaskNumberOfBlock).ToList();
        }

        public async Task<bool> UpdateTaskAsync(UpdateTaskDTO updateTask)
        {
            var task = await _context.BlocksTasks.FirstOrDefaultAsync(x => x.TaskId == updateTask.TaskId);

            task.TaskName = updateTask.TaskName;
            task.Duration = updateTask.Duration;
            task.Link = updateTask.Link;
            task.Description = updateTask.Description;

            _context.BlocksTasks.Update(task);

            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteTaskAsync(Guid taskId)
        {
            var task = await _context.BlocksTasks.FirstOrDefaultAsync(x => x.TaskId == taskId);
            _context.BlocksTasks.Remove(task);
            return await SaveChangesAsync();
        }

        public async Task<bool> AddTaskAsync(AddTaskDTO newTask)
        {
            int numberOfBlock = _context.BlocksTasks.AsNoTracking().Where(x => x.Block == newTask.Block).OrderByDescending(x => x.TaskNumberOfBlock).FirstOrDefault().TaskNumberOfBlock + 1;

            var task = new BlocksTask()
            {
                TaskName = newTask.TaskName,
                TaskDateCreated = DateTime.UtcNow,
                Duration = newTask.Duration,
                Block = newTask.Block,
                Link = newTask.Link,
                TaskNumberOfBlock = numberOfBlock,
                Description = newTask.Description
            };

            await _context.BlocksTasks.AddAsync(task);

            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }

        public async Task<TaskDTO> GetTaskByIdAsync(Guid taskId, Guid userId)
        {
            var task = await _context.BlocksTasks.AsNoTracking().FirstOrDefaultAsync(x => x.TaskId == taskId);

            var studyRepository = new StatusStudyRepository(_context);

            return new TaskDTO()
            {
                TaskId = task.TaskId,
                TaskName = task.TaskName,
                TaskDateCreated = task.TaskDateCreated,
                DurationNeeded = task.Duration,
                Link = task.Link,
                TaskNumberOfBlock = task.TaskNumberOfBlock,
                Description = task.Description,
                Status = await studyRepository.CheckStateByIdAsync(task.TaskId, userId),
                DateStart = await studyRepository.GetDateStart(task.TaskId, userId),
                Duration = await studyRepository.GetDuration(task.TaskId, userId),
            };
        }

        public async Task<bool> TaskComparisonByTitleAndBlockAsync(string title, Guid blockId)
        {
            return await _context.BlocksTasks.AnyAsync(x => x.Block == blockId && title == x.TaskName);
        }

        public async Task<Guid> GetBlockIdByTaskIdAsync(Guid taskId)
        {
            return _context.BlocksTasks.FirstOrDefault(x => x.TaskId == taskId).Block;
        }
    }
}
