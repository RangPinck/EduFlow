﻿using EduFlowApi.DTOs.BlockDTOs;
using EduFlowApi.DTOs.UserDTOs;

namespace EduFlowApi.DTOs.CourseDTOs
{
    public class CourseStatisticsDTO
    {
        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public AuthorOfCourseDTO Author { get; set; }

        public int CountBlocks { get; set; } = 0;

        public double ProcentOfCompletion { get; set; } = 0.0;

        public List<BlockStatisticsDTO> BlocksStatistics { get; set; }
    }
}
