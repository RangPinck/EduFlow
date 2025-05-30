using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class LogsPageVM : ViewModelBase
    {
        [ObservableProperty]
        private DateTime _selsectedDate = DateTime.Now;

        [ObservableProperty]
        private string _path;

        public async Task SaveFile()
        {
            var file = await MainWindowViewModel.ApiClient.GetLogs(DateOnly.FromDateTime(SelsectedDate), Path);

            //eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiYWRtaW5AYWRtaW4uY29tIiwiZ2l2ZW5fbmFtZSI6ImFkbWluYWRtaW5hZG1pbiIsInJvbGUiOiLQkNC00LzQuNC90LjRgdGC0YDQsNGC0L7RgCIsInVuaXF1ZV9uYW1lIjoiYWRtaW5AYWRtaW4uY29tIiwibmFtZWlkIjoiMzNiNTg0ODQtMWJmNS00YzQyLWJhNzItMmE1M2JiZjY3NTgxIiwiZW1haWwiOiJhZG1pbkBhZG1pbi5jb20iLCJuYmYiOjE3NDg1NzQ4MjcsImV4cCI6MTc0ODYxODAyNywiaWF0IjoxNzQ4NTc0ODI3LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDUzIiwiYXVkIjoic3R1ZHkuZWR1Rmxvdy5jbGllbnQifQ.k0noxQBfjXg-iP-ytNP58gJ4yl-lhw3PBkIVsFxoTKKv4VM8Ecj490-5iLdLynJ14jpoLjeCxbLYxgVclTcaRA

            if (file != null)
            {

            }
        }
    }
}
