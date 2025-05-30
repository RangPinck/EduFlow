using EduFlowApi.DTOs.UserDTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EduFlowApi.DTOs.CourseDTOs
{
    public class SubscribesUsersOfCourseDTO : INotifyPropertyChanged
    {
        public ObservableCollection<SubscribeUserDTO> UnSubscridedUsers { get; set; } = new ObservableCollection<SubscribeUserDTO>();

        public ObservableCollection<SubscribeUserDTO> SubscridedUsers { get; set; } = new ObservableCollection<SubscribeUserDTO>();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
