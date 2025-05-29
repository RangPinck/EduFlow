using EduFlowApi.DTOs.PracticeDTOs;
using EduFlowApi.DTOs.StudyStateDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace EduFlowApi.DTOs.TaskDTOs
{
    public class TaskDTO : INotifyPropertyChanged
    {
        private StudyStateDTO _status;

        public Guid TaskId { get; set; }

        public string TaskName { get; set; } = null!;

        public DateTime TaskDateCreated { get; set; }

        public int DurationNeeded { get; set; }

        public string? Link { get; set; }

        public int TaskNumberOfBlock { get; set; }

        public string? Description { get; set; }

        public virtual StudyStateDTO Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? DateStart { get; set; }

        public int Duration { get; set; }

        public List<PracticeDTO> Practics { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}