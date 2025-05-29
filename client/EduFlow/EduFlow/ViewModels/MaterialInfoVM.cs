using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.MaterialDTOs;
using EduFlowApi.DTOs.StudyStateDTOs;
using MsBox.Avalonia;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class MaterialInfoVM : ViewModelBase
    {
        [ObservableProperty]
        private MaterialDTO _material;

        private Guid _blockId;

        [ObservableProperty]
        private bool _isVisibleDuration = false;

        [ObservableProperty]
        private List<StudyStateDTO> _studies = new();

        public MaterialInfoVM() { }

        public MaterialInfoVM(MaterialDTO material, Guid blockId)
        {
            _material = material;

            GetStudyStatemens();

            if (Material.Status.StateId == 3)
            {
                IsVisibleDuration = true;
            }
        }

        private async Task GetStudyStatemens()
        {
            var response = await MainWindowViewModel.ApiClient.GetStudyStatmens();
            Studies = JsonConvert.DeserializeObject<List<StudyStateDTO>>(response)!;
        }

        public void GoToBack()
        {
            MainWindowViewModel.Instance.RegistratePageBefore(nameof(BlokPage));
            MainWindowViewModel.Instance.GoToPageBefore();
        }

        public void ChengeState()
        {
            Material.Status = Material.Status.StateId >= Studies.Count ? Studies[0] : Studies[Material.Status.StateId];

            if (Material.Status.StateId == 3)
            {
                IsVisibleDuration = true;
            }
            else
            {
                IsVisibleDuration = false;
            }
        }

        public async Task SaveState()
        {
            string response = await MainWindowViewModel.ApiClient.UpdateStatus(new UpdateStudyStateDTO()
            {
                UpdateObjectId = Material.BlockMaterialId,
                StateId = Material.Status.StateId,
                BlockId = _blockId,
                Duration = Material.Duration
            });

            if (string.IsNullOrEmpty(response))
            {
                return;
            }

            await MessageBoxManager.GetMessageBoxStandard("Статус материала", "Статус материала успешно обновлён!", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
        }
    }
}
