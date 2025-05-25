using Microsoft.EntityFrameworkCore;
using EduFlowApi.DTOs.PracticeDTOs;
using EduFlowApi.Interfaces;
using EduFlowApi.Models;

namespace EduFlowApi.Repositories
{
    public class PracticeRepository : IPracticeRepository
    {
        private readonly EduFlowDbContext _context;

        public PracticeRepository(EduFlowDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddPracticeAsync(AddPracticeDTO newPractice)
        {
            var lastPracticeTask = await _context.TasksPractices.AsNoTracking().OrderByDescending(x => x.NumberPracticeOfTask).FirstOrDefaultAsync();

            int numberPractice = 1;

            if (lastPracticeTask != null)
            {
                numberPractice = (int)lastPracticeTask.NumberPracticeOfTask + 1;
            }

            var practice = new TasksPractice()
            {
                PracticeName = newPractice.PracticeName,
                PracticeDateCreated = DateTime.UtcNow,
                Duration = newPractice.Duration,
                Link = newPractice.Link,
                Task = newPractice.Task,
                NumberPracticeOfTask = numberPractice
            };

            await _context.TasksPractices.AddAsync(practice);

            return await SaveChangesAsync();
        }

        public async Task<bool> UpdatePracticeAsync(UpdatePracticeDTO updatePractice)
        {
            var practice = await _context.TasksPractices.FirstOrDefaultAsync(x => x.PracticeId == updatePractice.PracticeId);

            practice.PracticeName = updatePractice.PracticeName;
            practice.Duration = updatePractice.Duration;
            practice.Link = updatePractice.Link;

            _context.TasksPractices.Update(practice);

            return await SaveChangesAsync();
        }

        public async Task<bool> DeletePracticeAsync(Guid practiceId)
        {
            var practice = await _context.TasksPractices.FirstOrDefaultAsync(x => x.PracticeId == practiceId);
            _context.TasksPractices.Remove(practice);
            return await SaveChangesAsync();
        }

        public async Task<PracticeDTO> GetPracticeByIdAsync(Guid practiceId, Guid userId)
        {
            var practice = await _context.TasksPractices.AsNoTracking().FirstOrDefaultAsync(x => x.PracticeId == practiceId);

            var studyRepository = new StatusStudyRepository(_context);

            return new PracticeDTO()
            {
                PracticeId = practice.PracticeId,
                PracticeName = practice.PracticeName,
                PracticeDateCreated = practice.PracticeDateCreated,
                DurationNeeded = practice.Duration,
                Link = practice.Link,
                Task = practice.Task,
                NumberPracticeOfTask = practice.NumberPracticeOfTask,
                Status = await studyRepository.CheckStateByIdAsync(practice.PracticeId, userId),
                DateStart = await studyRepository.GetDateStart(practice.PracticeId, userId),
                Duration = await studyRepository.GetDuration(practice.PracticeId, userId),
            };
        }

        public async Task<IEnumerable<PracticeDTO>> GetPracticsOfBlockIdAsync(Guid blockId, Guid userId)
        {
            List<Guid> tasksIds = await _context.BlocksTasks.Where(x => x.Block == blockId).Select(x => x.TaskId).ToListAsync();

            var tasks = await _context.TasksPractices.AsNoTracking().Where(x => tasksIds.Contains(x.Task)).ToListAsync();

            var studyRepository = new StatusStudyRepository(_context);

            List<PracticeDTO> results = new List<PracticeDTO>();

            foreach (var item in tasks)
            {
                results.Add(new PracticeDTO()
                {
                    PracticeId = item.PracticeId,
                    PracticeName = item.PracticeName,
                    PracticeDateCreated = item.PracticeDateCreated,
                    DurationNeeded = item.Duration,
                    Link = item.Link,
                    Task = item.Task,
                    NumberPracticeOfTask = item.NumberPracticeOfTask,
                    Status = await studyRepository.CheckStateByIdAsync(item.PracticeId, userId),
                    DateStart = await studyRepository.GetDateStart(item.PracticeId, userId),
                    Duration = await studyRepository.GetDuration(item.PracticeId, userId),
                });
            }

            return results;
        }

        public async Task<IEnumerable<PracticeDTO>> GetPracticsOfTaskIdAsync(Guid taskId, Guid userId)
        {
            var tasks = await _context.TasksPractices.AsNoTracking().Where(x => x.Task == taskId).ToListAsync();

            var studyRepository = new StatusStudyRepository(_context);

            List<PracticeDTO> results = new List<PracticeDTO>();

            foreach (var item in tasks)
            {
                results.Add(new PracticeDTO()
                {
                    PracticeId = item.PracticeId,
                    PracticeName = item.PracticeName,
                    PracticeDateCreated = item.PracticeDateCreated,
                    DurationNeeded = item.Duration,
                    Link = item.Link,
                    Task = item.Task,
                    NumberPracticeOfTask = item.NumberPracticeOfTask,
                    Status = await studyRepository.CheckStateByIdAsync(item.PracticeId, userId),
                    DateStart = await studyRepository.GetDateStart(item.PracticeId, userId),
                    Duration = await studyRepository.GetDuration(item.PracticeId, userId),
                });
            }

            return results;
        }

        public async Task<List<(Guid id, IEnumerable<PracticeDTO>)>> GetPracticsByTaskIds(List<Guid> tasksIds, Guid userId)
        {
            List<(Guid id, IEnumerable<PracticeDTO>)> practics = new List<(Guid id, IEnumerable<PracticeDTO>)>();

            foreach (var item in tasksIds)
            {
                practics.Add((item, await GetPracticsOfTaskIdAsync(item, userId)));
            }

            return practics;
        }

        public async Task<List<(Guid id, IEnumerable<PracticeDTO>)>> GetPracticsByBlockIds(List<Guid> blocksIds, Guid userId)
        {
            List<(Guid id, IEnumerable<PracticeDTO>)> practics = new List<(Guid id, IEnumerable<PracticeDTO>)>();

            foreach (var item in blocksIds)
            {
                practics.Add((item, await GetPracticsOfBlockIdAsync(item, userId)));
            }

            return practics;
        }

        public async Task<bool> SaveChangesAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }

        public async Task<bool> PracticeComparisonByTitleAndTaskAsync(string title, Guid taskId)
        {
            return await _context.TasksPractices.AnyAsync(x => x.Task == taskId && title == x.PracticeName);
        }
    }
}
