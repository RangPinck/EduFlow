using EduFlowApi.DTOs.BlockDTOs;
using EduFlowApi.DTOs.CourseDTOs;

namespace EduFlowApi.Interfaces
{
    public interface IBlockRepository
    {
        public Task<IEnumerable<ShortBlockDTO>> GetBlocksOfCourseAsync(Guid courseId);

        public Task<bool> AddBlockAsync(AddBlockDTO newBlock);

        public Task<bool> SaveChangesAsync();

        public Task<bool> BlockIsExistByTitleAsync(Guid courseId, string title);

        public Task<bool> BlockIsExistByTitleAsync(Guid courseId, string title, Guid updateBlockId);

        public Task<bool> BlockIsExistByIdAsync(Guid blockId);

        public Task<StandardCourseDTO> GetCourseByBlockIdAsync(Guid blockId);

        public Task<bool> UpdateBlockAsync(UpdateBlockDTO updateBlock);

        public Task<bool> CompletedTasksFromTheBlockIsExistsAsync(Guid blockId);

        public Task<bool> DeleteBlockAsync(Guid blockId);

        public Task<List<FullBlockDTO>> GetFullBlocksData(Guid courseId, Guid userId);
    }
}
