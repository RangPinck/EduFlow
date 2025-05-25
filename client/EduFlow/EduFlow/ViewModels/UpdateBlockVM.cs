using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.BlockDTOs;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class UpdateBlockVM : ViewModelBase
    {
        [ObservableProperty]
        private FullBlockDTO _block;

        [ObservableProperty]
        private string _header = "";

        [ObservableProperty]
        private bool _isEdit = false;

        private Guid _coursId = Guid.Empty;

        public UpdateBlockVM() { }

        public UpdateBlockVM(Guid coursId)
        {
            Block = new FullBlockDTO();
            _coursId = coursId;
            Header = "Создание блока";
        }

        public UpdateBlockVM(Guid coursId, FullBlockDTO block)
        {
            Block = new FullBlockDTO()
            {
                BlockId = block.BlockId,
                BlockName = block.BlockName,
                Description = block.Description
            };
            _coursId = coursId;
            Header = "Редатикарование блока";
            IsEdit = true;
        }

        public async Task DeleteBlock()
        {
            var result = await MessageBoxManager.GetMessageBoxStandard("Удаление блока", $"Вы действительно хотите удалить этот блок?", MsBox.Avalonia.Enums.ButtonEnum.YesNo).ShowAsync();

            if (result == MsBox.Avalonia.Enums.ButtonResult.Yes)
            {
                var response = await MainWindowViewModel.ApiClient.DeleteBlock(Block.BlockId);
                MainWindowViewModel.Instance.GoToPageBefore();
                IsEdit = false;
            }
        }

        public async Task GoToBack()
        {
            var result = await MessageBoxManager.GetMessageBoxStandard(Header, "Вы действительно хотите покинуть страницу? Произведённые изменения не сохраняться!", MsBox.Avalonia.Enums.ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Question).ShowAsync();

            if (result == ButtonResult.Yes)
            {
                MainWindowViewModel.Instance.GoToPageBefore();
            }
        }

        public async Task SaveData()
        {
            string result;

            if (_isEdit)
            {
                await MainWindowViewModel.ApiClient.UpdateBlock(new UpdateBlockDTO()
                {
                    BlockId = Block.BlockId,
                    BlockName = Block.BlockName,
                    Description = Block.Description
                });
                MainWindowViewModel.Instance.GoToPageBefore();
            }
            else
            {
                string response = await MainWindowViewModel.ApiClient.AddBlock(new AddBlockDTO()
                {
                    BlockName = Block.BlockName,
                    Description = Block.Description,
                    Course = _coursId
                });

                if (!string.IsNullOrEmpty(response))
                {
                    MainWindowViewModel.Instance.GoToPageBefore();
                }
            }
        }
    }
}
