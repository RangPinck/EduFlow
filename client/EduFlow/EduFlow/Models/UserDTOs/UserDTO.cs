using System;
using System.Collections.Generic;
using EduFlowApi.DTOs.CourseDTOs;

namespace EduFlowApi.DTOs.UserDTOs
{
    public class UserDTO
    {
        public Guid UserId { get; set; }

        public string UserLogin { get; set; } = string.Empty;

        public string UserSurname { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string? UserPatronymic { get; set; }

        public bool IsFirst { get; set; }

        public List<string> UserRole { get; set; } = new List<string>();

        public DateTime UserDataCreate { get; set; }

        public List<CourseStatisticsDTO> UserStatistics { get; set; }
    }
}
