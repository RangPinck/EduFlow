using EduFlowApi.DTOs.StudyStateDTOs;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EduFlowApi.DTOs.PracticeDTOs
{
    public class PracticeDTO : INotifyPropertyChanged
    {
        public Guid PracticeId { get; set; }

        public string PracticeName { get; set; } = null!;

        public DateTime PracticeDateCreated { get; set; }

        public int DurationNeeded { get; set; }

        public string? Link { get; set; }

        public Guid Task { get; set; }

        public int? NumberPracticeOfTask { get; set; }

        public DateTime? DateStart { get; set; }

        public int Duration { get; set; }

        private StudyStateDTO _status;

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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
