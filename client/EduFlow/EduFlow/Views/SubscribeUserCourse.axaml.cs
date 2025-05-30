using Avalonia.Controls;
using EduFlow.ViewModels;

namespace EduFlow;

public partial class SubscribeUserCourse : UserControl
{
    public SubscribeUserCourse()
    {
        InitializeComponent();
        DataContext = new SubscribeUserCourseVM();
    }
}