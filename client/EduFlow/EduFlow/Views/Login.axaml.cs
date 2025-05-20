using Avalonia.Controls;
using EduFlow.ViewModels;

namespace EduFlow;

public partial class Login : UserControl
{
    public Login()
    {
        InitializeComponent();
        DataContext = new LoginVM();
    }
}