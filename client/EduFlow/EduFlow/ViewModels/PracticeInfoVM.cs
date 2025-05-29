using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.PracticeDTOs;
using EduFlowApi.DTOs.StudyStateDTOs;
using EduFlowApi.DTOs.TaskDTOs;
using MsBox.Avalonia;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class PracticeInfoVM : ViewModelBase
    {
        [ObservableProperty]
        private PracticeDTO _practice;

        private TaskDTO _task;

        private Guid _blockId;

        [ObservableProperty]
        private bool _isVisibleDuration = false;

        [ObservableProperty]
        private List<StudyStateDTO> _studies = new();

        public PracticeInfoVM() { }

        public PracticeInfoVM(PracticeDTO practice, Guid blockId, TaskDTO task)
        {
            _practice = practice;
            _task = task;
            _blockId = blockId;

            GetStudyStatemens();

            if (Practice.Status.StateId == 3)
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
            MainWindowViewModel.Instance.PageContent = new TaskInfo(_task, _blockId);
        }

        public void ChengeState()
        {
            Practice.Status = Practice.Status.StateId >= Studies.Count ? Studies[0] : Studies[Practice.Status.StateId];

            if (Practice.Status.StateId == 3)
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
                UpdateObjectId = Practice.PracticeId,
                StateId = Practice.Status.StateId,
                BlockId = _blockId,
                Duration = Practice.Duration
            });

            if (string.IsNullOrEmpty(response))
            {
                return;
            }

            await MessageBoxManager.GetMessageBoxStandard("Статус практики", "Статус практики успешно обновлён!", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
        }
    }
}
