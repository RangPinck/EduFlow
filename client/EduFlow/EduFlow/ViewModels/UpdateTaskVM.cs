using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.TaskDTOs;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class UpdateTaskVM : ViewModelBase
    {
        [ObservableProperty]
        private TaskDTO _task;

        private Guid _block;

        [ObservableProperty]
        private string _header = "";

        [ObservableProperty]
        private bool _isEdit = false;

        public UpdateTaskVM() { }

        public UpdateTaskVM(Guid blockId)
        {
            _block = blockId;
            Task = new TaskDTO();
            Header = "Добавление задачи";
        }

        public UpdateTaskVM(TaskDTO task)
        {
            Task = task;
            Header = "Обновление задачи";
            IsEdit = true;
        }

        public async Task DeleteTask()
        {
            var result = await MessageBoxManager.GetMessageBoxStandard("Удаление задачи", $"Вы действительно хотите удалить эту задачу?", MsBox.Avalonia.Enums.ButtonEnum.YesNo).ShowAsync();

            if (result == MsBox.Avalonia.Enums.ButtonResult.Yes)
            {
                var response = await MainWindowViewModel.ApiClient.DeleteTask(Task.TaskId);
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
                result = await MainWindowViewModel.ApiClient.UpdateTask(new UpdateTaskDTO()
                {
                    TaskId = Task.TaskId,
                    TaskName = Task.TaskName,
                    Duration = Task.DurationNeeded,
                    Link = Task.Link,
                    Description = Task.Description
                });
            }
            else
            {
                result = await MainWindowViewModel.ApiClient.AddTask(new AddTaskDTO()
                {
                    Block = _block,
                    TaskName = Task.TaskName,
                    Duration = Task.DurationNeeded,
                    Link = Task.Link,
                    Description = Task.Description
                });
            }

            if (!string.IsNullOrEmpty(result))
            {
                MainWindowViewModel.Instance.GoToPageBefore();
            }
        }
    }
}
