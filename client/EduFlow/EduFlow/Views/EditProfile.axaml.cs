using Avalonia.Controls;
using EduFlow.ViewModels;
using EduFlowApi.DTOs.AuthDTO;

namespace EduFlow;

public partial class EditProfile : UserControl
{
    public EditProfile()
    {
        InitializeComponent();
        DataContext = new EditProfileVM();
    }

    public EditProfile(SignInDTO user)
    {
        InitializeComponent();
        DataContext = new EditProfileVM(user);
    }
}