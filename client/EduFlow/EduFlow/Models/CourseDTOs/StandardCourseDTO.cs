using System;
using System.Collections.Generic;

namespace EduFlowApi.DTOs.CourseDTOs
{
    public class StandardCourseDTO
    {
        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public DateTime CourseDataCreate { get; set; }

        public string? Description { get; set; }

        public string? Link { get; set; }

        public Guid Author { get; set; }
    }
}
