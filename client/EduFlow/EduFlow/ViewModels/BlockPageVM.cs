using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.BlockDTOs;
using EduFlowApi.DTOs.CourseDTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class BlockPageVM : ViewModelBase
    {
        [ObservableProperty]
        private List<ShortBlockDTO> _blocks = new();

        [ObservableProperty]
        private ShortCourseDTO _course = new();

        public BlockPageVM() { }

        public BlockPageVM(ShortCourseDTO course)
        {
            _course = course;
            GetBlocks(course);
        }

        private async Task GetBlocks(ShortCourseDTO currentCourse)
        {
            var response = await MainWindowViewModel.ApiClient.GetBlock(currentCourse);
            Blocks = JsonConvert.DeserializeObject<List<ShortBlockDTO>>(response);
        }

        public void ToCourses()
        {
            MainWindowViewModel.Instance.PageContent = new CoursesPage();
        }

        public void AddBlock()
        {
            //MainWindowViewModel.Instance.PageContent = new AddEditCourse();
        }

        public async Task EditBlock(ShortBlockDTO block)
        {
            if (block is null)
            {
                await MainWindowViewModel.ErrorMessage("Блоки", "Для совершения действия выберите блок, нажав на него!");
                return;
            }

            //MainWindowViewModel.Instance.PageContent = new AddEditCourse(Item);
        }
    }
}
