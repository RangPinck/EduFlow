using Avalonia.Controls;
using EduFlow.ViewModels;

namespace EduFlow;

public partial class UserPage : UserControl
{
    public UserPage()
    {
        InitializeComponent();
        DataContext = new UsersPageVM();
    }
}