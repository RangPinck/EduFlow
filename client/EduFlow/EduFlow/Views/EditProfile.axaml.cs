using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;
using EduFlow.ViewModels;
using EduFlowApi.DTOs.AuthDTO;
using EduFlowApi.DTOs.UserDTOs;
using System;

namespace EduFlow;

public partial class EditProfile : UserControl
{
    public EditProfile()
    {
        InitializeComponent();
        DataContext = new EditProfileVM();
    }

    public EditProfile(UserControl latestPage)
    {
        InitializeComponent();
        DataContext = new EditProfileVM(latestPage);
    }

    public EditProfile(SignInDTO user, UserControl latestPage)
    {
        InitializeComponent();
        DataContext = new EditProfileVM(user, latestPage);
    }
}