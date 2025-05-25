using EduFlowApi.DTOs.MaterialDTOs;
using EduFlowApi.DTOs.PracticeDTOs;
using EduFlowApi.DTOs.TaskDTOs;

namespace EduFlowApi.DTOs.BlockDTOs
{
    public class FullBlockDTO
    {
        public Guid BlockId { get; set; }

        public string BlockName { get; set; } = null!;

        public DateTime BlockDateCreated { get; set; }

        public string? Description { get; set; }

        public int? BlockNumberOfCourse { get; set; }

        public int FullyCountTask { get; set; }

        public int FullyDurationNeeded { get; set; }

        public int CompletedTaskCount { get; set; }

        public int DurationCompletedTask { get; set; }

        public double PercentCompletedTask { get; set; }

        public double PercentDurationCompletedTask { get; set; }

        public IEnumerable<TaskDTO> Tasks { get; set; }

        public IEnumerable<MaterialDTO> Materials { get; set; }
    }
}
