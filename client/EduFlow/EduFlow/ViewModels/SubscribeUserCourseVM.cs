using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.CourseDTOs;
using EduFlowApi.DTOs.TaskDTOs;
using EduFlowApi.DTOs.UserDTOs;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class SubscribeUserCourseVM : ViewModelBase
    {
        [ObservableProperty]
        private string _header = string.Empty;

        [ObservableProperty]
        private SubscribesUsersOfCourseDTO _users;

        public SubscribeUserCourseVM()
        {
            Header = MainWindowViewModel.Instance.SelectedCourse.CourseName;

            Init();

        }

        public async Task Init()
        {
            await GetUsersForSubscribeCourse();

            OnPropertyChanged(nameof(Users));
        }

        public async Task GetUsersForSubscribeCourse()
        {
            var response = await MainWindowViewModel.ApiClient.GetUsersForSubscribeCourse(MainWindowViewModel.Instance.SelectedCourse.CourseId);
            Users = JsonConvert.DeserializeObject<SubscribesUsersOfCourseDTO>(response);
        }

        public void GoToBack()
        {
            MainWindowViewModel.Instance.RegistratePageBefore(nameof(BlokPage));
            MainWindowViewModel.Instance.GoToPageBefore();
        }

        public async Task SubscribeUserOfCourse(SubscribeUserDTO user)
        {
            string result = await MainWindowViewModel.ApiClient.SubscribeUserForACourse(new SubscribeUserCourseDTO()
            {
                userId = user.UserId,
                courseId = MainWindowViewModel.Instance.SelectedCourse.CourseId,
            });

            if (!string.IsNullOrEmpty(result))
            {
                Users.UnSubscridedUsers.Remove(user);
                Users.SubscridedUsers.Add(user);
            }
        }

        public async Task UnsubscribeUserOfCourse(SubscribeUserDTO user)
        {
            string result = await MainWindowViewModel.ApiClient.UnsubscribeUserForACourse(new SubscribeUserCourseDTO()
            {
                userId = user.UserId,
                courseId = MainWindowViewModel.Instance.SelectedCourse.CourseId,
            });

            if (!string.IsNullOrEmpty(result))
            {
                Users.UnSubscridedUsers.Add(user);
                Users.SubscridedUsers.Remove(user);
            }
        }
    }
}
