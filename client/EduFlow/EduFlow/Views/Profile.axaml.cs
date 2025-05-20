using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EduFlow.ViewModels;

namespace EduFlow;

public partial class Profile : UserControl
{
    public Profile()
    {
        InitializeComponent();
        DataContext = new ProfileVM();
    }
}