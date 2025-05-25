using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.CourseDTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class CoursesPageVM : ViewModelBase
    {
        [ObservableProperty]
        private List<ShortCourseDTO> _courses = new();

        [ObservableProperty]
        private bool _isAdminKurator = false;

        public CoursesPageVM()
        {
            GetCourses();
            IsAdminKurator = MainWindowViewModel.Instance.IsAdminKurator;
        }

        public void AddCourse()
        {
            MainWindowViewModel.Instance.PageContent = new UpdateCourse();
        }

        public async Task EditCourse(ShortCourseDTO course)
        {
            if (course is null)
            {
                await MainWindowViewModel.ErrorMessage("Курсы", "Для совершения действия выберите курс, нажав на него!");
                return;
            }

            MainWindowViewModel.Instance.RegistratePageBefore(nameof(CoursesPage));
            MainWindowViewModel.Instance.PageContent = new UpdateCourse(course);
        }

        private async Task GetCourses()
        {
            var response = await MainWindowViewModel.ApiClient.GetAllCourses();
            Courses = JsonConvert.DeserializeObject<List<ShortCourseDTO>>(response);
        }

        public async Task ToBlockByCourses(ShortCourseDTO course)
        {
            if (course is null)
            {
                await MainWindowViewModel.ErrorMessage("Курсы", "Для совершения действия выберите курс, нажав на него!");
                return;
            }

            MainWindowViewModel.Instance.RegistratePageBefore(nameof(CoursesPage));
            MainWindowViewModel.Instance.PageContent = new BlokPage(course);
        }
    }
}
