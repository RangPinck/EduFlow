using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.BlockDTOs;
using EduFlowApi.DTOs.CourseDTOs;
using EduFlowApi.DTOs.MaterialDTOs;
using EduFlowApi.DTOs.StudyStateDTOs;
using EduFlowApi.DTOs.TaskDTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class BlockPageVM : ViewModelBase
    {
        [ObservableProperty]
        private List<StudyStateDTO> _studies = new();

        [ObservableProperty]
        private FullCourseDTO _course = new();

        [ObservableProperty]
        private bool _isAdminKurator = false;

        public BlockPageVM() { }

        public BlockPageVM(ShortCourseDTO course)
        {
            IsAdminKurator = MainWindowViewModel.Instance.IsAdminKurator;
            MainWindowViewModel.Instance.SelectedCourse = course;
            Init(course);
        }

        private async Task Init(ShortCourseDTO course)
        {
            await GetStudyStatemens();
            await GetFullCoursData(course.CourseId, MainWindowViewModel.User.Id);
        }

        private async Task GetFullCoursData(Guid courseId, Guid userId)
        {
            var response = await MainWindowViewModel.ApiClient.GetFullCoursData(courseId, userId);
            Course = JsonConvert.DeserializeObject<FullCourseDTO>(response);
        }

        private async Task GetStudyStatemens()
        {
            var response = await MainWindowViewModel.ApiClient.GetStudyStatmens();
            Studies = JsonConvert.DeserializeObject<List<StudyStateDTO>>(response);
        }

        public void GoToBack()
        {
            MainWindowViewModel.Instance.RegistratePageBefore(nameof(CoursesPage));
            MainWindowViewModel.Instance.GoToPageBefore();
        }

        public void AddBlock()
        {
            MainWindowViewModel.Instance.RegistratePageBefore(nameof(BlokPage));
            MainWindowViewModel.Instance.PageContent = new UpdateBlock(Course.CourseId);
        }

        public async Task EditBlock(FullBlockDTO block)
        {
            if (block is null)
            {
                await MainWindowViewModel.ErrorMessage("Блоки", "Для совершения действия выберите блок, нажав на него!");
                return;
            }

            MainWindowViewModel.Instance.RegistratePageBefore(nameof(BlokPage));
            MainWindowViewModel.Instance.PageContent = new UpdateBlock(Course.CourseId, block);
        }

        public async Task GoToBlockInfo(FullBlockDTO block)
        {
            if (block is null)
            {
                await MainWindowViewModel.ErrorMessage("Блоки", "Для совершения действия выберите блок, нажав на него!");
                return;
            }

            MainWindowViewModel.Instance.RegistratePageBefore(nameof(BlokPage));

            MainWindowViewModel.Instance.PageContent = new BlockInfo(block, Course.CourseId);
        }

        public async Task AddMaterial(FullBlockDTO block)
        {
            if (block is null)
            {
                await MainWindowViewModel.ErrorMessage("Блоки", "Для совершения действия выберите блок, нажав на него!");
                return;
            }

            MainWindowViewModel.Instance.RegistratePageBefore(nameof(BlokPage));
            MainWindowViewModel.Instance.PageContent = new UpdateMaterial(block.BlockId);
        }

        public async Task EditMaterial(MaterialDTO material)
        {
            if (material is null)
            {
                await MainWindowViewModel.ErrorMessage("Блоки", "Для совершения действия выберите материал, нажав на него!");
                return;
            }

            MainWindowViewModel.Instance.RegistratePageBefore(nameof(BlokPage));
            MainWindowViewModel.Instance.PageContent = new UpdateMaterial(material);
        }

        public async Task AddTask(FullBlockDTO block)
        {
            if (block is null)
            {
                await MainWindowViewModel.ErrorMessage("Блоки", "Для совершения действия выберите блок, нажав на него!");
                return;
            }

            MainWindowViewModel.Instance.RegistratePageBefore(nameof(BlokPage));
            MainWindowViewModel.Instance.PageContent = new UpdateTask(block.BlockId);
        }

        public async Task EditTask(TaskDTO task)
        {
            if (task is null)
            {
                await MainWindowViewModel.ErrorMessage("Блоки", "Для совершения действия выберите задачу, нажав на неё!");
                return;
            }

            MainWindowViewModel.Instance.RegistratePageBefore(nameof(BlokPage));
            MainWindowViewModel.Instance.PageContent = new UpdateTask(task);
        }

        public async Task GoToTask(TaskDTO task)
        {
            if (task is null)
            {
                await MainWindowViewModel.ErrorMessage("Задача", "Для совершения действия выберите задачу, нажав на неё!");
                return;
            }

            MainWindowViewModel.Instance.RegistratePageBefore(nameof(BlokPage));
            MainWindowViewModel.Instance.PageContent = new TaskInfo(task, Course.Blocks.First(x => x.Tasks.Contains(task)).BlockId);
        }
    }
}
