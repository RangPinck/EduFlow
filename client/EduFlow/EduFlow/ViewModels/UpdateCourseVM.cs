using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.CourseDTOs;
using EduFlowApi.DTOs.UserDTOs;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class UpdateCourseVM : ViewModelBase
    {
        [ObservableProperty]
        private AddCourseDTO _courseFull = new();

        [ObservableProperty]
        private UpdateCourseDTO _courseUpdate = new();

        [ObservableProperty]
        private int _authorIndex = 0;

        [ObservableProperty]
        private string _header = string.Empty;

        [ObservableProperty]
        private bool _isEdit = false;

        [ObservableProperty]
        private List<UserDTO> _users = new();

        public UpdateCourseVM()
        {
            CourseFull = new AddCourseDTO();
            GetAuthors(MainWindowViewModel.User.Token);
            Header = "Добавление курса";
        }

        public UpdateCourseVM(ShortCourseDTO course)
        {
            CourseUpdate.CourseId = course.CourseId;
            CourseFull.Title = course.CourseName;
            CourseFull.Link = course.Link;
            CourseFull.Description = course.Description;
            GetAuthors(MainWindowViewModel.User.Token);
            _isEdit = true;
            Header = $"Изменение курса \"{CourseFull.Title}\"";
        }

        private async Task GetAuthors(string token)
        {
            var response = await MainWindowViewModel.ApiClient.GetCourses();
            Users = JsonConvert.DeserializeObject<List<UserDTO>>(response);
            Users = [new UserDTO() { UserSurname = "Не выбрано", UserName = string.Empty, UserPatronymic = string.Empty }, .. Users];
            AuthorIndex = Users.IndexOf(Users.First(x => x.UserId == MainWindowViewModel.User.Id));
        }

        public async Task SaveData()
        {
            string result;

            if (_isEdit)
            {
                CourseUpdate.Title = CourseFull.Title;
                CourseUpdate.Link = CourseFull.Link;
                CourseUpdate.Description = CourseFull.Description;
                await MainWindowViewModel.ApiClient.UpdateCourse(CourseUpdate);
                MainWindowViewModel.Instance.PageContent = new CoursesPage();
            }
            else
            {
                if (AuthorIndex == 0)
                {
                    MainWindowViewModel.ErrorMessage(Header, "Выберите создателя курса!");
                }

                CourseFull.Author = Users[AuthorIndex].UserId;

                string response = await MainWindowViewModel.ApiClient.AddCourse(CourseFull);

                if (!string.IsNullOrEmpty(response))
                {
                    MainWindowViewModel.Instance.PageContent = new CoursesPage();
                }
            }
        }

        public async Task GoToBack()
        {
            var result = await MessageBoxManager.GetMessageBoxStandard(Header, "Вы действительно хотите покинуть страницу? Произведённые изменения не сохраняться!", MsBox.Avalonia.Enums.ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Question).ShowAsync();

            if (result == ButtonResult.Yes)
            {
                MainWindowViewModel.Instance.PageContent = new CoursesPage();
            }
        }

        public async Task DeleteCourse()
        {
            var result = await MessageBoxManager.GetMessageBoxStandard("Удаление курса", $"Вы действительно хотите удалить курс {CourseUpdate.Title}?", MsBox.Avalonia.Enums.ButtonEnum.YesNo).ShowAsync();

            if (result == MsBox.Avalonia.Enums.ButtonResult.Yes)
            {
                var response = await MainWindowViewModel.ApiClient.DeleteCourse(CourseUpdate.CourseId);
                MainWindowViewModel.Instance.PageContent = new CoursesPage();
            }
        }
    }
}
