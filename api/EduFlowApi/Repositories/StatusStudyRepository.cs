using EduFlowApi.DTOs.StudyStateDTOs;
using EduFlowApi.Interfaces;
using EduFlowApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EduFlowApi.Repositories
{
    public class StatusStudyRepository : IStatusStudyRepository
    {
        private readonly EduFlowDbContext _context;

        public StatusStudyRepository(EduFlowDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckMaterialByIdAsync(Guid materialId)
        {
            return await _context.BlocksMaterials.AsNoTracking().AnyAsync(x => x.BmId == materialId);
        }

        public async Task<bool> CheckPracticeByIdAsync(Guid practiceId)
        {
            return await _context.TasksPractices.AsNoTracking().AnyAsync(x => x.PracticeId == practiceId);
        }

        public async Task<int> GetDuration(Guid id, Guid userId)
        {
            bool taskExist = await CheckTaskByIdAsync(id);
            bool masterialExist = await CheckMaterialByIdAsync(id);
            bool practiceExist = await CheckPracticeByIdAsync(id);

            UsersTask duration = null;

            if (taskExist)
            {
                duration = await _context.UsersTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Task != null && x.Task == id && x.AuthUser == userId);
                return duration == null ? 0 : duration.DurationTask;
            }

            if (masterialExist)
            {
                duration = await _context.UsersTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Material != null && x.Material == id && x.AuthUser == userId);
                return duration == null ? 0 : duration.DurationMaterial;
            }

            if (practiceExist)
            {
                duration = await _context.UsersTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Practice != null && x.Practice == id && x.AuthUser == userId);
                return duration == null ? 0 : duration.DurationPractice;
            }

            return 0;
        }

        public async Task<DateTime?> GetDateStart(Guid id, Guid userId)
        {
            bool taskExist = await CheckTaskByIdAsync(id);
            bool masterialExist = await CheckMaterialByIdAsync(id);
            bool practiceExist = await CheckPracticeByIdAsync(id);

            UsersTask dateStart = null;

            if (taskExist)
            {
                dateStart = await _context.UsersTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Task != null && x.Task == id && x.AuthUser == userId);
            }

            if (masterialExist)
            {
                dateStart = await _context.UsersTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Material != null && x.Material == id && x.AuthUser == userId);
            }

            if (practiceExist)
            {
                dateStart = await _context.UsersTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Practice != null && x.Practice == id && x.AuthUser == userId);
            }

            return dateStart == null ? null : dateStart.DateStart;
        }

        public async Task<StudyStateDTO> CheckStateByIdAsync(Guid id, Guid userId)
        {
            bool taskExist = await CheckTaskByIdAsync(id);
            bool masterialExist = await CheckMaterialByIdAsync(id);
            bool practiceExist = await CheckPracticeByIdAsync(id);

            UsersTask state = null;

            if (taskExist)
            {
                state = await _context.UsersTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Task != null && x.Task == id && x.AuthUser == userId);
            }

            if (masterialExist)
            {
                state = await _context.UsersTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Material != null && x.Material == id && x.AuthUser == userId);
            }

            if (practiceExist)
            {
                state = await _context.UsersTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Practice != null && x.Practice == id && x.AuthUser == userId);
            }

            return new StudyStateDTO()
            {
                StateId = state != null ? state.Status : 1,
                StateName = state != null ? _context.StudyStates.AsNoTracking().First(x => x.StateId == state.Status).StateName : _context.StudyStates.AsNoTracking().First(x => x.StateId == 1).StateName,
            };
        }

        public async Task<List<(Guid id, StudyStateDTO state)>> GetStatusesByIdsAsync(List<Guid> ids, Guid userId)
        {
            var result = new List<(Guid id, StudyStateDTO state)>();

            foreach (var item in ids)
            {
                result.Add((item, await CheckStateByIdAsync(item, userId)));
            }

            return result;
        }

        public async Task<bool> CheckTaskByIdAsync(Guid taskId)
        {
            return await _context.BlocksTasks.AsNoTracking().AnyAsync(x => x.TaskId == taskId);
        }

        public async Task<IEnumerable<StudyStateDTO>> GetStudyStatesAsync()
        {
            return await _context.StudyStates.AsNoTracking().Select(x => new StudyStateDTO()
            {
                StateId = x.StateId,
                StateName = x.StateName
            }).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }

        public async Task<bool> UpdateStateAsync(UpdateStudyStateDTO state, Guid userId)
        {
            var task = await _context.BlocksTasks.AsNoTracking().FirstOrDefaultAsync(x => x.TaskId == state.UpdateObjectId);
            var practice = await _context.TasksPractices.AsNoTracking().FirstOrDefaultAsync(x => x.PracticeId == state.UpdateObjectId);
            var material = await _context.BlocksMaterials.AsNoTracking().FirstOrDefaultAsync(x => x.BmId == state.UpdateObjectId);

            var tableRow = await _context.UsersTasks.AsNoTracking().FirstOrDefaultAsync(x => x.AuthUser == userId && ((x.Task != null && x.Task == state.UpdateObjectId) || (x.Practice != null && x.Practice == state.UpdateObjectId) || (x.Material != null && x.Material == state.UpdateObjectId)));

            switch (state.StateId)
            {
                case 1:
                    _context.UsersTasks.Remove(tableRow);
                    break;
                case 2:
                    if (tableRow == null)
                    {
                        await _context.UsersTasks.AddAsync(GenerateRegistrationModel(task, practice, material, userId));
                    }
                    else
                    {
                        tableRow.Practice = practice != null ? practice.PracticeId : null;
                        tableRow.Material = material != null ? material.BmId : null;
                        tableRow.Task = task != null ? task.TaskId : null;
                        tableRow.Status = 2;
                        tableRow.DateStart = DateTime.UtcNow;
                        _context.UsersTasks.Update(tableRow);
                    }
                    break;
                case 3:
                    if (tableRow == null)
                    {
                        var newState = GenerateRegistrationModel(task, practice, material, userId);
                        newState.DurationMaterial = material != null ? state.Duration : 0;
                        newState.DurationTask = task != null ? state.Duration : 0;
                        newState.Status = 3;
                        newState.DurationPractice = practice != null ? state.Duration : 0;
                        await _context.UsersTasks.AddAsync(newState);
                    }
                    else
                    {
                        tableRow.Practice = practice != null ? practice.PracticeId : null;
                        tableRow.Material = material != null ? material.BmId : null;
                        tableRow.Task = task != null ? task.TaskId : null;
                        tableRow.Status = 3;
                        tableRow.DurationMaterial = material != null ? state.Duration : 0;
                        tableRow.DurationTask = task != null ? state.Duration : 0;
                        tableRow.DurationPractice = practice != null ? state.Duration : 0;
                        tableRow.DateStart = DateTime.UtcNow;
                        _context.UsersTasks.Update(tableRow);
                    }
                    break;
            }

            return await SaveChangesAsync();
        }

        private UsersTask GenerateRegistrationModel(BlocksTask? task, TasksPractice? practice, BlocksMaterial? material, Guid userId)
        {
            return new UsersTask()
            {
                Practice = practice != null ? practice.PracticeId : null,
                Material = material != null ? material.BmId : null,
                Task = task != null ? task.TaskId : null,
                Status = 2,
                DateStart = DateTime.UtcNow,
                AuthUser = userId
            };
        }
    }
}
