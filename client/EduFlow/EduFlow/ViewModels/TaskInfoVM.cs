using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.StudyStateDTOs;
using EduFlowApi.DTOs.TaskDTOs;
using MsBox.Avalonia;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class TaskInfoVM : ViewModelBase
    {
        [ObservableProperty]
        private TaskDTO _task = new();

        private Guid _blockId;

        [ObservableProperty]
        private List<StudyStateDTO> _studies = new();

        [ObservableProperty]
        private bool _isVisibleDuration = false;

        public TaskInfoVM() { }

        public TaskInfoVM(TaskDTO task, Guid blockId)
        {
            _blockId = blockId;
            Init(task);
        }

        private async Task Init(TaskDTO task)
        {
            await GetStudyStatemens();

            if (Studies != null)
            {
                _task = task;
                OnPropertyChanged(nameof(Task));
                if (Task.Status.StateId == 3)
                {
                    IsVisibleDuration = true;
                }
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
            Task.Status = Task.Status.StateId >= Studies.Count ? Studies[0] : Studies[Task.Status.StateId];

            if (Task.Status.StateId == 3)
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
            await MainWindowViewModel.ApiClient.UpdateStatus(new UpdateStudyStateDTO()
            {
                UpdateObjectId = Task.TaskId,
                StateId = Task.Status.StateId,
                BlockId = _blockId,
                Duration = Task.Duration
            });

            await MessageBoxManager.GetMessageBoxStandard("Статус задачи", "Статус задачи успешно обновлён!", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
        }
    }
}
