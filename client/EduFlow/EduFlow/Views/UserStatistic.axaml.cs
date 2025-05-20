using Avalonia.Controls;
using EduFlow.ViewModels;
using System;
using System.Threading.Tasks;

namespace EduFlow;

public partial class UserStatistic : UserControl
{
    public UserStatistic()
    {
        InitializeComponent();
        DataContext = new UserStatisticVM();
    }

    public UserStatistic(Guid userId, UserControl latesPage)
    {
        InitializeComponent();
        DataContext = new UserStatisticVM(userId, latesPage);
    }
}