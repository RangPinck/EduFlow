using Avalonia.Controls;
using EduFlow.ViewModels;
using EduFlowApi.DTOs.CourseDTOs;

namespace EduFlow;

public partial class BlokPage : UserControl
{
    public BlokPage()
    {
        InitializeComponent();
        DataContext = new BlockPageVM();
    }

    public BlokPage(ShortCourseDTO course)
    {
        InitializeComponent();
        DataContext = new BlockPageVM(course);
    }
}