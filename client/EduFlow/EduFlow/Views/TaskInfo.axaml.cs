using Avalonia.Controls;
using EduFlow.ViewModels;
using EduFlowApi.DTOs.TaskDTOs;

namespace EduFlow;

public partial class TaskInfo : UserControl
{
    public TaskInfo()
    {
        InitializeComponent();
        DataContext = new TaskInfoVM();
    }

    public TaskInfo(TaskDTO task)
    {
        InitializeComponent();
        DataContext = new TaskInfoVM(task);
    }
}