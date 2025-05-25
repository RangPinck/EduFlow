using EduFlowApi.DTOs.BlockDTOs;
using EduFlowApi.DTOs.CourseDTOs;
using EduFlowApi.DTOs.UserDTOs;
using EduFlowApi.Interfaces;
using EduFlowApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EduFlowApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EduFlowDbContext _context;
        private readonly UserManager<AuthUser> _userManager;

        public UserRepository(EduFlowDbContext context, UserManager<AuthUser> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ICollection<UserDTO>> GetAllUsersAsync(bool isAdmin = false)
        {
            try
            {
                var users = await _userManager.Users.AsNoTracking().ToListAsync();
                var profiles = await _context.Users.Include(x => x.UserCourses).Include(x => x.UsersTasks).AsNoTracking().ToListAsync();

                List<UserDTO> result = new List<UserDTO>();

                if (isAdmin)
                {
                    result = users.Select(user => new UserDTO()
                    {
                        UserId = user.Id,
                        UserLogin = user.Email,
                        UserSurname = profiles.FirstOrDefault(x => x.UserId == user.Id).UserSurname,
                        UserName = profiles.FirstOrDefault(x => x.UserId == user.Id).UserName,
                        UserPatronymic = profiles.FirstOrDefault(x => x.UserId == user.Id).UserPatronymic,
                        UserDataCreate = user.UserDataCreate,
                        UserRole = _userManager.GetRolesAsync(user).Result.ToList()
                    }).ToList();

                    for (global::System.Int32 i = 0; i < result.Count; i++)
                    {
                        result[i].UserStatistics = await GetUserCoursStatisticById(result[i].UserId);
                    }

                    //.CoursesBlocks.Select(block => new BlockStatisticsDTO()
                    // {
                    //     BlockId = block.BlockId,

                    //     BlockName = block.BlockName,

                    //     FullyCountTask = block.BlocksTasks.Count() + block.BlocksMaterials.Where(mat => mat.Duration != null).Count() + block.BlocksTasks.Select(x => x.TasksPractices.Count()).Sum(),

                    //     FullyDurationNeeded = block.BlocksTasks.Select(x => x.Duration).Sum() + block.BlocksMaterials.Where(mat => mat.Duration != null).Select(x => Convert.ToInt32(x.Duration)).Sum() + block.BlocksTasks.Select(x => x.TasksPractices.Select(x => Convert.ToInt32(x.Duration)).Sum()).Sum(),

                    //     CompletedTaskCount = _context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Count(),

                    //     DurationCompletedTask = _context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Sum(x => x.DurationMaterial + x.DurationPractice + x.DurationTask),

                    //     PercentCompletedTask = Math.Round((double)_context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Count() / (double)(block.BlocksTasks.Count() + block.BlocksMaterials.Where(mat => mat.Duration != null).Count() + block.BlocksTasks.Select(x => x.TasksPractices.Count()).Sum()) * 100.0, 2),

                    //     PercentDurationCompletedTask = Math.Round((double)_context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Sum(x => x.DurationMaterial + x.DurationPractice + x.DurationTask) / (double)(block.BlocksTasks.Select(x => x.Duration).Sum() + block.BlocksMaterials.Where(mat => mat.Duration != null).Select(x => Convert.ToInt32(x.Duration)).Sum() + block.BlocksTasks.Select(x => x.TasksPractices.Select(x => Convert.ToInt32(x.Duration)).Sum()).Sum()) * 100.0, 2),
                    // })
                }
                else
                {
                    result = users.Select(user => new UserDTO()
                    {
                        UserId = user.Id,
                        UserLogin = user.Email,
                        UserSurname = profiles.FirstOrDefault(x => x.UserId == user.Id).UserSurname,
                        UserName = profiles.FirstOrDefault(x => x.UserId == user.Id).UserName,
                        UserPatronymic = profiles.FirstOrDefault(x => x.UserId == user.Id).UserPatronymic,
                        UserDataCreate = user.UserDataCreate,
                        UserRole = _userManager.GetRolesAsync(user).Result.ToList(),

                        //UserStatistics = _context.CoursesBlocks.Select(block => new BlockStatisticsDTO()
                        //{
                        //    BlockId = block.BlockId,

                        //    BlockName = block.BlockName,

                        //    FullyCountTask = block.BlocksTasks.Count() + block.BlocksMaterials.Where(mat => mat.Duration != null).Count() + block.BlocksTasks.Select(x => x.TasksPractices.Count()).Sum(),

                        //    FullyDurationNeeded = block.BlocksTasks.Select(x => x.Duration).Sum() + block.BlocksMaterials.Where(mat => mat.Duration != null).Select(x => Convert.ToInt32(x.Duration)).Sum() + block.BlocksTasks.Select(x => x.TasksPractices.Select(x => Convert.ToInt32(x.Duration)).Sum()).Sum(),

                        //    CompletedTaskCount = _context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Count(),

                        //    DurationCompletedTask = _context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Sum(x => x.DurationMaterial + x.DurationPractice + x.DurationTask),

                        //    PercentCompletedTask = Math.Round((double)_context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Count() / (double)(block.BlocksTasks.Count() + block.BlocksMaterials.Where(mat => mat.Duration != null).Count() + block.BlocksTasks.Select(x => x.TasksPractices.Count()).Sum()) * 100.0, 2),

                        //    PercentDurationCompletedTask = Math.Round((double)_context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Sum(x => x.DurationMaterial + x.DurationPractice + x.DurationTask) / (double)(block.BlocksTasks.Select(x => x.Duration).Sum() + block.BlocksMaterials.Where(mat => mat.Duration != null).Select(x => Convert.ToInt32(x.Duration)).Sum() + block.BlocksTasks.Select(x => x.TasksPractices.Select(x => Convert.ToInt32(x.Duration)).Sum()).Sum()) * 100.0, 2),
                        //}).ToList()

                    }).Where(x => x.UserRole.Contains("Ученик")).ToList();

                    for (global::System.Int32 i = 0; i < result.Count; i++)
                    {
                        result[i].UserStatistics = await GetUserCoursStatisticById(result[i].UserId);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return new List<UserDTO>();
            }
        }

        public async Task<bool> UserIsExistAsync(Guid userId)
        {
            return await _context.Users.AnyAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<CourseAuthorDTO>> GetAuthorsForCoursesAsync()
        {
            List<Guid> users = _userManager.GetUsersInRoleAsync("Администратор").Result.Select(x => x.Id).ToList();
            users = [.. users, .. _userManager.GetUsersInRoleAsync("Куратор").Result.Select(x => x.Id).Distinct().ToList()];

            return await _context.Users.AsNoTracking().Where(x => users.Contains(x.UserId)).Select(x => new CourseAuthorDTO()
            {
                UserId = x.UserId,
                UserName = x.UserName,
                UserSurname = x.UserSurname,
                UserPatronymic = x.UserPatronymic
            }).ToListAsync();
        }

        public async Task<List<CourseStatisticsDTO>> GetUserCoursStatisticById(Guid userId)
        {
            List<CourseStatisticsDTO> coursesStistics = new List<CourseStatisticsDTO>();

            List<Course> courses = await _context.Courses
                .AsNoTracking()
                .Include(x => x.CoursesBlocks)
                .ThenInclude(x => x.BlocksTasks)
                .ThenInclude(x => x.TasksPractices)
                .ThenInclude(x => x.UsersTasks)
                .Include(x => x.CoursesBlocks)
                .ThenInclude(x => x.BlocksMaterials)
                .ThenInclude(x => x.MaterialNavigation)
                .Include(x => x.UserCourses)
                .Include(x => x.AuthorNavigation)
                .Where(x => x.UserCourses.Any(x => x.UserId == userId))
                .ToListAsync();

            if (courses != null && courses.Count > 0)
            {
                foreach (var item in courses)
                {
                    var blocksStatistic = (await new BlockRepository(_context)
                        .GetFullBlocksData(item.CourseId, userId))
                        .Select(x => new BlockStatisticsDTO()
                        {
                            BlockId = x.BlockId,
                            BlockName = x.BlockName,
                            FullyCountTask = x.FullyCountTask,
                            FullyDurationNeeded = x.FullyDurationNeeded,
                            CompletedTaskCount = x.CompletedTaskCount,
                            DurationCompletedTask = x.DurationCompletedTask,
                            PercentCompletedTask = x.PercentCompletedTask,
                            PercentDurationCompletedTask = x.PercentDurationCompletedTask
                        }).ToList();

                    coursesStistics.Add(new CourseStatisticsDTO()
                    {
                        CourseId = item.CourseId,
                        CourseName = item.CourseName,
                        Author = new AuthorOfCourseDTO()
                        {
                            UserSurname = item.AuthorNavigation.UserSurname,
                            UserName = item.AuthorNavigation.UserName,
                            UserPatronymic = item.AuthorNavigation.UserPatronymic
                        },
                        CountBlocks = item.CoursesBlocks.Count,
                        BlocksStatistics = blocksStatistic,
                        ProcentOfСompletion = blocksStatistic.Count,
                    });
                }
            }
            else
            {
                return null;
            }

            return coursesStistics;
        }

        public async Task<UserDTO> GetLogginedUserWithStatisticsAsync(Guid userId)
        {
            try
            {
                var users = await _userManager.Users.AsNoTracking().ToListAsync();
                var profiles = await _context.Users.Include(x => x.UserCourses).Include(x => x.UsersTasks).AsNoTracking().ToListAsync();

                List<UserDTO> result = new List<UserDTO>();

                result = users.Select(user => new UserDTO()
                {
                    UserId = user.Id,
                    UserLogin = user.Email,
                    UserSurname = profiles.FirstOrDefault(x => x.UserId == user.Id).UserSurname,
                    UserName = profiles.FirstOrDefault(x => x.UserId == user.Id).UserName,
                    UserPatronymic = profiles.FirstOrDefault(x => x.UserId == user.Id).UserPatronymic,
                    UserDataCreate = user.UserDataCreate,
                    UserRole = _userManager.GetRolesAsync(user).Result.ToList(),

                    //UserStatistics = 

                }).Where(x => x.UserId == userId).ToList();

                for (global::System.Int32 i = 0; i < result.Count; i++)
                {
                    result[i].UserStatistics = await GetUserCoursStatisticById(result[i].UserId);
                }

                //_context.CoursesBlocks.Select(block => new BlockStatisticsDTO()
                //{
                //    BlockId = block.BlockId,

                //    BlockName = block.BlockName,

                //    FullyCountTask = block.BlocksTasks.Count() + block.BlocksMaterials.Where(mat => mat.Duration != null).Count() + block.BlocksTasks.Select(x => x.TasksPractices.Count()).Sum(),

                //    FullyDurationNeeded = block.BlocksTasks.Select(x => x.Duration).Sum() + block.BlocksMaterials.Where(mat => mat.Duration != null).Select(x => Convert.ToInt32(x.Duration)).Sum() + block.BlocksTasks.Select(x => x.TasksPractices.Select(x => Convert.ToInt32(x.Duration)).Sum()).Sum(),

                //    CompletedTaskCount = _context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Count(),

                //    DurationCompletedTask = _context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Sum(x => x.DurationMaterial + x.DurationPractice + x.DurationTask),

                //    PercentCompletedTask = Math.Round((double)_context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Count() / (double)(block.BlocksTasks.Count() + block.BlocksMaterials.Where(mat => mat.Duration != null).Count() + block.BlocksTasks.Select(x => x.TasksPractices.Count()).Sum()) * 100.0, 2),

                //    PercentDurationCompletedTask = Math.Round((double)_context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Sum(x => x.DurationMaterial + x.DurationPractice + x.DurationTask) / (double)(block.BlocksTasks.Select(x => x.Duration).Sum() + block.BlocksMaterials.Where(mat => mat.Duration != null).Select(x => Convert.ToInt32(x.Duration)).Sum() + block.BlocksTasks.Select(x => x.TasksPractices.Select(x => Convert.ToInt32(x.Duration)).Sum()).Sum()) * 100.0, 2),
                //}).ToList()

                return result[0];
            }
            catch (Exception ex)
            {
                return new UserDTO();
            }
        }
    }
}
