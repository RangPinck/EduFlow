using EduFlowApi.DTOs.UserDTOs;

namespace EduFlowApi.DTOs.CourseDTOs
{
    public class SubscribesUsersOfCourseDTO
    {
        public List<SubscribeUserDTO> UnSubscridedUsers { get; set; } = new List<SubscribeUserDTO>();

        public List<SubscribeUserDTO> SubscridedUsers { get; set; } = new List<SubscribeUserDTO>();
    }
}
