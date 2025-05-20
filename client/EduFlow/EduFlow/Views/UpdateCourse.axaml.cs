using Avalonia.Controls;
using EduFlow.ViewModels;
using EduFlowApi.DTOs.CourseDTOs;

namespace EduFlow;

public partial class UpdateCourse : UserControl
{
    public UpdateCourse()
    {
        InitializeComponent();
        DataContext = new UpdateCourseVM();
    }

    public UpdateCourse(ShortCourseDTO course)
    {
        InitializeComponent();
        DataContext = new UpdateCourseVM(course);
    }
}