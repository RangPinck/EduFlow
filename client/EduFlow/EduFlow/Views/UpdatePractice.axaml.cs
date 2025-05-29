using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EduFlow.ViewModels;
using EduFlowApi.DTOs.PracticeDTOs;
using EduFlowApi.DTOs.TaskDTOs;
using System;
using System.Threading.Tasks;

namespace EduFlow;

public partial class UpdatePractice : UserControl
{
    public UpdatePractice()
    {
        InitializeComponent();
        DataContext = new UpdatePracticeVM();
    }

    public UpdatePractice(TaskDTO task, Guid blockId)
    {
        InitializeComponent();
        DataContext = new UpdatePracticeVM(task, blockId);
    }

    public UpdatePractice(PracticeDTO practice, Guid blockId,TaskDTO task)
    {
        InitializeComponent();
        DataContext = new UpdatePracticeVM(practice, blockId, task);
    }
}