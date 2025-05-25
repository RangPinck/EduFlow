using EduFlowApi.DTOs.StudyStateDTOs;

namespace EduFlowApi.Interfaces
{
    public interface IStatusStudyRepository
    {
        public Task<IEnumerable<StudyStateDTO>> GetStudyStatesAsync();

        public Task<bool> UpdateStateAsync(UpdateStudyStateDTO state, Guid userId);

        public Task<bool> CheckTaskByIdAsync(Guid taskId);

        public Task<bool> CheckPracticeByIdAsync(Guid practiceId);

        public Task<bool> CheckMaterialByIdAsync(Guid materialId);

        public Task<StudyStateDTO> CheckStateByIdAsync(Guid id, Guid userId);

        public Task<bool> SaveChangesAsync();
    }
}
