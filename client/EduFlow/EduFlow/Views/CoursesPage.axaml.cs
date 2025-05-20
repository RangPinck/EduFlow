using Avalonia.Controls;
using EduFlow.ViewModels;

namespace EduFlow;

public partial class CoursesPage : UserControl
{
    public CoursesPage()
    {
        InitializeComponent();
        DataContext = new CoursesPageVM();
    }
}