using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EduFlow.ViewModels;

namespace EduFlow;

public partial class BlockInfo : UserControl
{
    public BlockInfo()
    {
        InitializeComponent();
        DataContext = new BlockInfoVM();
    }
}