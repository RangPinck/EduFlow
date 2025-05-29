using EduFlowApi.DTOs.StudyStateDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace EduFlowApi.DTOs.MaterialDTOs
{
    public class MaterialDTO : INotifyPropertyChanged
    {
        public Guid MaterialId { get; set; }

        public Guid BlockMaterialId { get; set; }

        public string MaterialName { get; set; } = null!;

        public DateTime MaterialDateCreate { get; set; }

        public string? Link { get; set; }

        private int _typeId { get; set; }

        private string _typeName { get; set; } = null!;

        public int TypeId
        {
            get => _typeId;
            set
            {
                if (_typeId != value)
                {
                    _typeId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TypeName
        {
            get => _typeName;
            set
            {
                if (_typeName != value)
                {
                    _typeName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Description { get; set; }

        public StudyStateDTO Status { get; set; }

        public int? DurationNeeded { get; set; } = 0;

        public DateTime? DateStart { get; set; }

        public int? Duration { get; set; }

        public string? Note { get; set; }

        public DateTime BmDateCreate { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
