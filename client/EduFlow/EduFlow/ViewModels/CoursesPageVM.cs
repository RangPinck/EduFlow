using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.CourseDTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class CoursesPageVM : ViewModelBase
    {
        [ObservableProperty]
        private List<ShortCourseDTO> _courses = new();

        public CoursesPageVM()
        {
            GetCourses();
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

            MainWindowViewModel.Instance.PageContent = new UpdateCourse(course);
        }

        private async Task GetCourses()
        {
            var response = await MainWindowViewModel.ApiClient.GetAllCourses(MainWindowViewModel.User.Token);
            Courses = JsonConvert.DeserializeObject<List<ShortCourseDTO>>(response);
        }

        public async Task ToBlockByCourses(ShortCourseDTO course)
        {
            if (course is null)
            {
                await MainWindowViewModel.ErrorMessage("Курсы", "Для совершения действия выберите курс, нажав на него!");
                return;
            }

            MainWindowViewModel.Instance.PageContent = new BlokPage(course);
        }
    }
}
