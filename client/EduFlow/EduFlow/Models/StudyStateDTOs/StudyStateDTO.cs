using CommunityToolkit.Mvvm.ComponentModel;
using EduFlow.ViewModels;
using System;
using System.Collections.Generic;

namespace EduFlowApi.DTOs.StudyStateDTOs
{
    public class StudyStateDTO
    {
        public int StateId { get; set; }

        public string StateName { get; set; } = null!;
    }
}
