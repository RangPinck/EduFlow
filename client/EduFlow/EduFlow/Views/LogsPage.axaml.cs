using Avalonia.Controls;
using EduFlow.ViewModels;

namespace EduFlow;

public partial class LogsPage : UserControl
{
    public LogsPage()
    {
        InitializeComponent();
        DataContext = new LogsPageVM();
    }
}