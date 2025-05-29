using Avalonia.Controls;
using EduFlow.ViewModels;
using EduFlowApi.DTOs.MaterialDTOs;
using System;

namespace EduFlow;

public partial class MaterialInfo : UserControl
{
    public MaterialInfo()
    {
        InitializeComponent();
        DataContext = new MaterialInfoVM();
    }

    public MaterialInfo(MaterialDTO material, Guid blockId)
    {
        InitializeComponent();
        DataContext = new MaterialInfoVM(material, blockId);
    }
}