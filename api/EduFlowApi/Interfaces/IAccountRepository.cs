using EduFlowApi.DTOs.AccountDTOs;
using EduFlowApi.DTOs.UserDTOs;

namespace EduFlowApi.Interfaces
{
    public interface IAccountRepository
    {
        public Task<bool> UpdateUserProfileAsync(UpdateProfileDTO updateProfile);

        public Task<bool> SaveChangesAsync();

        public Task<UserDTO> GetUserDataByIdAsync(Guid userId);

        public Task<bool> RegistrationUserFirstLoginAsync(Guid userId);
    }
}
