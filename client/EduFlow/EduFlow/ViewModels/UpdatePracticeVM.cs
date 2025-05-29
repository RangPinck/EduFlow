using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.PracticeDTOs;
using EduFlowApi.DTOs.TaskDTOs;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class UpdatePracticeVM : ViewModelBase
    {
        [ObservableProperty]
        private PracticeDTO _practice;

        private TaskDTO _task;

        private Guid _blockId;

        [ObservableProperty]
        private string _header = "";

        [ObservableProperty]
        private bool _isEdit = false;

        public UpdatePracticeVM() { }

        public UpdatePracticeVM(TaskDTO task, Guid blockId)
        {
            _task = task;
            Practice = new PracticeDTO();
            Header = "Добавление практики";
        }

        public UpdatePracticeVM(PracticeDTO practice, Guid blockId, TaskDTO task)
        {
            _task = task;
            Practice = practice;
            Header = "Обновление практики";
            IsEdit = true;
        }

        public async Task DeletePractice()
        {
            var result = await MessageBoxManager.GetMessageBoxStandard("Удаление практики", $"Вы действительно хотите удалить эту практику?", MsBox.Avalonia.Enums.ButtonEnum.YesNo).ShowAsync();

            if (result == MsBox.Avalonia.Enums.ButtonResult.Yes)
            {
                var response = await MainWindowViewModel.ApiClient.DeletePractice(Practice.PracticeId);

                if (string.IsNullOrEmpty(response))
                {
                    return;
                }

                int id = _task.Practics.IndexOf(_task.Practics.First(x => x.PracticeId == Practice.PracticeId));
                _task.Practics.RemoveAt(id);

                IsEdit = false;
                MainWindowViewModel.Instance.PageContent = new TaskInfo(_task, _blockId);
            }
        }

        public async Task GoToBack()
        {
            var result = await MessageBoxManager.GetMessageBoxStandard(Header, "Вы действительно хотите покинуть страницу? Произведённые изменения не сохраняться!", MsBox.Avalonia.Enums.ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Question).ShowAsync();

            if (result == ButtonResult.Yes)
            {
                MainWindowViewModel.Instance.PageContent = new TaskInfo(_task, _blockId);
            }
        }

        public async Task SaveData()
        {
            string result;

            if (_isEdit)
            {
                result = await MainWindowViewModel.ApiClient.UpdatePractice(new UpdatePracticeDTO()
                {
                    PracticeId = Practice.PracticeId,
                    PracticeName = Practice.PracticeName,
                    Duration = Practice.DurationNeeded,
                    Link = Practice.Link
                });
            }
            else
            {
                result = await MainWindowViewModel.ApiClient.AddPractice(new AddPracticeDTO()
                {
                    PracticeName = Practice.PracticeName,
                    Duration = Practice.DurationNeeded,
                    Link = Practice.Link,
                    Task = _task.TaskId
                });
            }

            if (!string.IsNullOrEmpty(result))
            {
                if (IsEdit)
                {
                    int id = _task.Practics.IndexOf(_task.Practics.First(x => x.PracticeId == Practice.PracticeId));
                    _task.Practics.RemoveAt(id);
                }

                _task.Practics.Add(Practice);

                MainWindowViewModel.Instance.PageContent = new TaskInfo(_task, _blockId);
            }
        }
    }
}
