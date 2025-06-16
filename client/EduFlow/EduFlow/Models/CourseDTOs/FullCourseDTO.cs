using EduFlowApi.DTOs.BlockDTOs;
using EduFlowApi.DTOs.UserDTOs;
using System;
using System.Collections.Generic;

namespace EduFlowApi.DTOs.CourseDTOs
{
    public class FullCourseDTO
    {
        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public DateTime CourseDataCreate { get; set; }

        public string? Description { get; set; }

        public string? Link { get; set; }

        public AuthorOfCourseDTO Author { get; set; }

        public int CountBlocks { get; set; } = 0;

        public double ProcentOf�ompletion { get; set; } = 0.0;

        public List<FullBlockDTO> Blocks { get; set; }
    }
}
