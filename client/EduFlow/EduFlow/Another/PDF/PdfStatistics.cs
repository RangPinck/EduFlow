using EduFlowApi.DTOs.UserDTOs;
using MsBox.Avalonia;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EduFlow.Another.PDF
{
    public static class PdfStatistics
    {
        public static async Task CreatePDF(UserDTO user, string path)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .AlignCenter()
                        .Text("Отчёт по пользователю")
                        .SemiBold().FontSize(18).FontColor(Colors.Blue.Darken3);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Component(new UserInfoComponent(user));
                            });

                            if (user.UserStatistics == null)
                            {

                            }
                            else
                            {
                                column.Item().PaddingTop(15).Component(new StatisticsComponent(user));
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Страница ");
                            x.CurrentPageNumber();
                            x.Span(" из ");
                            x.TotalPages();
                        });
                });
            })
            .GeneratePdf();

            string fullPath = Path.Combine(path, $"Статистика_{user.UserSurname}_{user.UserRole[0]}_{DateOnly.FromDateTime(DateTime.Now):yyyy_MM_dd}.pdf");

            await MessageBoxManager.GetMessageBoxStandard("Сохранение статискики", "Статискика сохранена по пути: " + fullPath, MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();

            await System.IO.File.WriteAllBytesAsync(fullPath, pdfBytes);
        }
    }

    public class UserInfoComponent : IComponent
    {
        private readonly UserDTO _user;

        public UserInfoComponent(UserDTO user)
        {
            _user = user;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text("Основная информация").Bold().FontSize(14);
                column.Item().PaddingTop(5).Text($"ФИО: {_user.UserSurname} {_user.UserName} {_user.UserPatronymic}");
                column.Item().Text($"Логин: {_user.UserLogin}");
                column.Item().Text($"Дата регистрации: {_user.UserDataCreate:dd.MM.yyyy}");
                column.Item().Text($"Первичный вход: {(_user.IsFirst ? "Да" : "Нет")}");
                column.Item().Text($"Роль: {_user.UserRole[0]}");
            });
        }
    }

    public class StatisticsComponent : IComponent
    {
        private readonly UserDTO _user;

        public StatisticsComponent(UserDTO user)
        {
            _user = user;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().PaddingBottom(10).Text("Статистика по курсам").Bold().FontSize(16);

                foreach (var course in _user.UserStatistics)
                {
                    column.Item().PaddingBottom(10).Background(Colors.Grey.Lighten3).Padding(10).Column(courseColumn =>
                    {
                        courseColumn.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"Курс: {course.CourseName}").SemiBold();
                            row.AutoItem().Text($"Прогресс: {course.ProcentOfСompletion}%").SemiBold();
                        });

                        courseColumn.Item().Text($"Автор: {course.Author.UserSurname} {course.Author.UserName} {course.Author.UserPatronymic}");
                        courseColumn.Item().Text($"Количество блоков: {course.CountBlocks}");

                        courseColumn.Item().PaddingTop(10).Text("Статистика по блокам:").FontSize(12).Bold();

                        courseColumn.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Блок").Bold();
                                header.Cell().Text("Завершено задач").Bold();
                                header.Cell().Text("Пройдено времени").Bold();
                                header.Cell().Text("Прогресс").Bold();
                            });

                            foreach (var block in course.BlocksStatistics)
                            {
                                table.Cell().Text(block.BlockName);
                                table.Cell().Text($"{block.CompletedTaskCount}/{block.FullyCountTask}");
                                table.Cell().Text($"{block.DurationCompletedTask} мин / {block.FullyDurationNeeded} мин");
                                table.Cell().Text($"{block.PercentCompletedTask}%");
                            }
                        });
                    });
                }
            });
        }
    }
}
