using Avalonia.Controls;
using EduFlow.ViewModels;
using EduFlowApi.DTOs.MaterialDTOs;
using System;

namespace EduFlow;

public partial class UpdateMaterial : UserControl
{
    public UpdateMaterial()
    {
        InitializeComponent();
        DataContext = new UpdateMaterialVM();
    }

    public UpdateMaterial(Guid blockId)
    {
        InitializeComponent();
        DataContext = new UpdateMaterialVM(blockId);
    }

    public UpdateMaterial(MaterialDTO material)
    {
        InitializeComponent();
        DataContext = new UpdateMaterialVM(material);
    }
}