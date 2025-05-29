using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.MaterialDTOs;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class UpdateMaterialVM : ViewModelBase
    {
        [ObservableProperty]
        private MaterialDTO _material;

        [ObservableProperty]
        private int _materialTypeId;

        [ObservableProperty]
        private ObservableCollection<MaterialTypeDTO> _materialTypes;

        private Guid _block;

        [ObservableProperty]
        private string _header = "";

        [ObservableProperty]
        private bool _isEdit = false;

        public UpdateMaterialVM() { }

        public UpdateMaterialVM(Guid blockId)
        {
            _block = blockId;
            Material = new MaterialDTO();
            Header = "Добавление материала";
            GetMaterialsTypes();
        }

        public UpdateMaterialVM(MaterialDTO material)
        {
            _material = material;
            Header = "Обновление материала";
            IsEdit = true;
            GetMaterialsTypes();
        }

        private async Task GetMaterialsTypes()
        {
            var response = await MainWindowViewModel.ApiClient.GetMaterialsTypes();
            MaterialTypes = JsonConvert.DeserializeObject<ObservableCollection<MaterialTypeDTO>>(response);
            MaterialTypeId = MaterialTypes.IndexOf(MaterialTypes.First(x => x.TypeId == Material.TypeId));
            OnPropertyChanged(nameof(MaterialTypeId));
        }

        public async Task DeleteMaterial()
        {
            var result = await MessageBoxManager.GetMessageBoxStandard("Удаление материала", $"Вы действительно хотите удалить эту материал?", MsBox.Avalonia.Enums.ButtonEnum.YesNo).ShowAsync();

            if (result == MsBox.Avalonia.Enums.ButtonResult.Yes)
            {
                var response = await MainWindowViewModel.ApiClient.DeleteMaterial(Material.MaterialId);
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

            Material.TypeId = MaterialTypes[MaterialTypeId].TypeId;

            if (_isEdit)
            {
                result = await MainWindowViewModel.ApiClient.UpdateMaterial(new UpdateMaterialDTO()
                {
                    MaterialId = Material.MaterialId,
                    MaterialName = Material.MaterialName,
                    Link = Material.Link,
                    TypeId = Material.TypeId,
                    Description = Material.Description,
                    Duration = Material.DurationNeeded,
                    Note = Material.Note
                });
            }
            else
            {
                result = await MainWindowViewModel.ApiClient.AddMaterial(new AddMaterialDTO()
                {
                    Block = _block,
                    MaterialName = Material.MaterialName,
                    Link = Material.Link,
                    TypeId = Material.TypeId,
                    Description = Material.Description,
                    Duration = Material.DurationNeeded,
                    Note = Material.Note
                });
            }

            if (!string.IsNullOrEmpty(result))
            {
                MainWindowViewModel.Instance.GoToPageBefore();
            }
        }
    }
}
