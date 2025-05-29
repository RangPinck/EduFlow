using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EduFlow.ViewModels;
using EduFlowApi.DTOs.TaskDTOs;
using System;

namespace EduFlow;

public partial class UpdateTask : UserControl
{
    public UpdateTask()
    {
        InitializeComponent();
        DataContext = new UpdateTaskVM();
    }

    public UpdateTask(Guid blockId)
    {
        InitializeComponent();
        DataContext = new UpdateTaskVM(blockId);
    }

    public UpdateTask(TaskDTO task)
    {
        InitializeComponent();
        DataContext = new UpdateTaskVM(task);
    }
}