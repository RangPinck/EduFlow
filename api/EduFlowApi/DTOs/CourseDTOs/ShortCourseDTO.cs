using EduFlowApi.DTOs.UserDTOs;

namespace EduFlowApi.DTOs.CourseDTOs
{
    public class ShortCourseDTO
    {
        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public DateTime CourseDataCreate { get; set; }

        public string? Description { get; set; }

        public string? Link { get; set; }

        public AuthorOfCourseDTO Author { get; set; }
    }
}
