using Avalonia.Controls;
using EduFlow.ViewModels;
using EduFlowApi.DTOs.TaskDTOs;
using System;

namespace EduFlow;

public partial class TaskInfo : UserControl
{
    public TaskInfo()
    {
        InitializeComponent();
        DataContext = new TaskInfoVM();
    }

    public TaskInfo(TaskDTO task, Guid blockId)
    {
        InitializeComponent();
        DataContext = new TaskInfoVM(task, blockId);
    }
}