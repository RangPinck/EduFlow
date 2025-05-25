using Avalonia.Controls;
using EduFlow.ViewModels;
using EduFlowApi.DTOs.BlockDTOs;
using System;

namespace EduFlow;

public partial class UpdateBlock : UserControl
{
    public UpdateBlock()
    {
        InitializeComponent();
        DataContext = new UpdateBlockVM();
    }

    public UpdateBlock(Guid coursId)
    {
        InitializeComponent();
        DataContext = new UpdateBlockVM(coursId);
    }

    public UpdateBlock(Guid coursId, FullBlockDTO block)
    {
        InitializeComponent();
        DataContext = new UpdateBlockVM(coursId, block);
    }
}