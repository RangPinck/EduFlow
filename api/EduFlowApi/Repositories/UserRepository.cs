using EduFlowApi.DTOs.BlockDTOs;
using EduFlowApi.DTOs.CourseDTOs;
using EduFlowApi.DTOs.UserDTOs;
using EduFlowApi.Interfaces;
using EduFlowApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                        UserRole = _userManager.GetRolesAsync(user).Result.ToList()

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
                }).Where(x => x.UserId == userId).ToList();

                for (global::System.Int32 i = 0; i < result.Count; i++)
                {
                    result[i].UserStatistics = await GetUserCoursStatisticById(result[i].UserId);
                }

                return result[0];
            }
            catch (Exception ex)
            {
                return new UserDTO();
            }
        }

        public async Task<SubscribesUsersOfCourseDTO> GetSubscribesUsersOfCourse(Guid courseId, bool isAdmin = false)
        {
            try
            {
                SubscribesUsersOfCourseDTO users = new SubscribesUsersOfCourseDTO();

                var authUsers = await _userManager.Users.AsNoTracking().ToListAsync();
                var profiles = await _context.Users.Include(x => x.UserCourses).AsNoTracking().ToListAsync();
                var usersFromCourse = await _context.UserCourses.Where(x => x.CourseId == courseId).ToListAsync();


                users.SubscridedUsers = authUsers.Select(user => new SubscribeUserDTO()
                {
                    UserId = user.Id,
                    UserSurname = profiles.FirstOrDefault(x => x.UserId == user.Id).UserSurname,
                    UserName = profiles.FirstOrDefault(x => x.UserId == user.Id).UserName,
                    UserPatronymic = profiles.FirstOrDefault(x => x.UserId == user.Id).UserPatronymic,
                    UserRole = _userManager.GetRolesAsync(user).Result.ToList()[0],
                }).Where(x => usersFromCourse.Any(y => y.UserId == x.UserId)).ToList();

                users.UnSubscridedUsers = authUsers.Select(user => new SubscribeUserDTO()
                {
                    UserId = user.Id,
                    UserSurname = profiles.FirstOrDefault(x => x.UserId == user.Id).UserSurname,
                    UserName = profiles.FirstOrDefault(x => x.UserId == user.Id).UserName,
                    UserPatronymic = profiles.FirstOrDefault(x => x.UserId == user.Id).UserPatronymic,
                    UserRole = _userManager.GetRolesAsync(user).Result.ToList()[0],
                }).Where(x => !usersFromCourse.Any(y => y.UserId == x.UserId)).ToList();

                if (!isAdmin)
                {
                    users.SubscridedUsers = users.SubscridedUsers.Where(x => x.UserRole == "Ученик").ToList();

                    users.UnSubscridedUsers = users.UnSubscridedUsers.Where(x => x.UserRole == "Ученик").ToList();
                }

                return users;
            }
            catch (Exception ex)
            {
                return new SubscribesUsersOfCourseDTO();
            }
        }
    }
}
