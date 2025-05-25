using Avalonia.Controls;
using EduFlow.ViewModels;
using EduFlowApi.DTOs.BlockDTOs;
using System;

namespace EduFlow;

public partial class BlockInfo : UserControl
{
    public BlockInfo()
    {
        InitializeComponent();
        DataContext = new BlockInfoVM();
    }

    public BlockInfo(FullBlockDTO block, Guid courseId)
    {
        InitializeComponent();
        DataContext = new BlockInfoVM(block, courseId);
    }
}