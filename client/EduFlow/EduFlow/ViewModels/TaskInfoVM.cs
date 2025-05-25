using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.CourseDTOs;
using EduFlowApi.DTOs.StudyStateDTOs;
using EduFlowApi.DTOs.TaskDTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class TaskInfoVM : ViewModelBase
    {
        [ObservableProperty]
        private TaskDTO _task;

        [ObservableProperty]
        private List<StudyStateDTO> _studies = new();

        public TaskInfoVM() { }

        public TaskInfoVM(TaskDTO task)
        {
            _task = task;
        }

        private async Task Init(ShortCourseDTO course)
        {
            await GetStudyStatemens();
        }

        private async Task GetStudyStatemens()
        {
            var response = await MainWindowViewModel.ApiClient.GetStudyStatmens();
            Studies = JsonConvert.DeserializeObject<List<StudyStateDTO>>(response);
        }

        public void GoToBack()
        {
            MainWindowViewModel.Instance.RegistratePageBefore(nameof(BlokPage));
            MainWindowViewModel.Instance.GoToPageBefore();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);


        }
    }
}
