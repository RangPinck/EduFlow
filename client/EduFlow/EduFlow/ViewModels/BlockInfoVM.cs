using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.BlockDTOs;
using System;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class BlockInfoVM : ViewModelBase
    {
        [ObservableProperty]
        private FullBlockDTO _block;

        private Guid _courseId;

        [ObservableProperty]
        private bool _isAdminKurator = false;

        public BlockInfoVM() { }

        public BlockInfoVM(FullBlockDTO block, Guid courseId)
        {
            Block = block;
            _courseId = courseId;
            IsAdminKurator = MainWindowViewModel.Instance.IsAdminKurator;
        }

        public void GoToBack()
        {
            MainWindowViewModel.Instance.RegistratePageBefore(nameof(BlokPage));
            MainWindowViewModel.Instance.GoToPageBefore();
        }

        public async Task EditBlock()
        {
            MainWindowViewModel.Instance.RegistratePageBefore(nameof(BlokPage));
            MainWindowViewModel.Instance.PageContent = new UpdateBlock(_courseId, Block);
        }
    }
}
