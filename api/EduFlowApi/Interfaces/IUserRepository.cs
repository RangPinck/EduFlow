using EduFlowApi.DTOs.UserDTOs;

namespace EduFlowApi.Interfaces
{
    public interface IUserRepository
    {
        public Task<ICollection<UserDTO>> GetAllUsersAsync(bool isAdmin = false);

        public Task<UserDTO> GetLogginedUserWithStatisticsAsync(Guid userId);

        public Task<IEnumerable<CourseAuthorDTO>> GetAuthorsForCoursesAsync();
        
        public Task<bool> UserIsExistAsync(Guid userId);
    }
}
