using Microsoft.EntityFrameworkCore;
using EduFlowApi.DTOs.AccountDTOs;
using EduFlowApi.DTOs.UserDTOs;
using EduFlowApi.Interfaces;
using EduFlowApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace EduFlowApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly EduFlowDbContext _context;

        public AccountRepository(EduFlowDbContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> GetUserDataByIdAsync(Guid userId)
        {
            var user = await _context.Users.AsNoTracking().FirstAsync(x => x.UserId == userId);
            return new UserDTO()
            {
                UserId = userId,
                UserName = user.UserName,
                UserSurname = user.UserName,
                UserPatronymic = user.UserName,
                IsFirst = user.IsFirst
            };
        }

        public async Task<bool> RegistrationUserFirstLoginAsync(Guid userId)
        {
            var profile = await _context.Users.FirstAsync(x => x.UserId == userId);

            profile.IsFirst = false;

            _context.Users.Update(profile);

            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }

        public async Task<bool> UpdateUserProfileAsync(UpdateProfileDTO updateProfile)
        {

            var profile = await _context.Users.FirstAsync(x => x.UserId == updateProfile.UserId);

            profile.UserName = updateProfile.UserName;
            profile.UserSurname = updateProfile.UserSurname;
            profile.UserPatronymic = updateProfile.UserPatronymic;

            _context.Users.Update(profile);

            return await SaveChangesAsync();
        }
    }
}
