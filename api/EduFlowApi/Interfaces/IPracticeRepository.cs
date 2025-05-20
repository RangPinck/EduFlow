using EduFlowApi.DTOs.PracticeDTOs;
using EduFlowApi.DTOs.TaskDTOs;

namespace EduFlowApi.Interfaces
{
    public interface IPracticeRepository
    {
        public Task<IEnumerable<PracticeDTO>> GetPracticsOfBlockIdAsync(Guid blockId, Guid userId);

        public Task<bool> UpdatePracticeAsync(UpdatePracticeDTO updatePractice);

        public Task<bool> DeletePracticeAsync(Guid taskId);

        public Task<bool> AddPracticeAsync(AddPracticeDTO newPractice);

        public Task<bool> SaveChangesAsync();

        public Task<PracticeDTO> GetPracticeByIdAsync(Guid practiceId, Guid userId);

        public Task<bool> PracticeComparisonByTitleAndTaskAsync(string title, Guid taskId);

        //public Task<Guid> GetBlockIdByTaskIdAsync(Guid taskId);
    }
}
