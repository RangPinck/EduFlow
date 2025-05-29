using Avalonia.Controls;
using EduFlow.ViewModels;
using EduFlowApi.DTOs.PracticeDTOs;
using EduFlowApi.DTOs.TaskDTOs;
using System;

namespace EduFlow;

public partial class PracticeInfo : UserControl
{
    public PracticeInfo()
    {
        InitializeComponent();
        DataContext = new PracticeInfoVM();
    }

    public PracticeInfo(PracticeDTO practice, Guid blockId, TaskDTO task)
    {
        InitializeComponent();
        DataContext = new PracticeInfoVM(practice, blockId, task);
    }
}