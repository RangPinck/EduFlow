using EduFlowApi.DTOs.RoleDTOs;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EduFlow.Models.Account
{
    public class FullUserDTO
    {
        public Guid UserId { get; set; }

        public bool IsFirst { get; set; }

        [Required(ErrorMessage = "Не указана фамилия!")]
        [Display(Name = "Surname")]
        [DefaultValue("")]
        public string UserSurname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Не указано имя!")]
        [Display(Name = "Name")]
        [DefaultValue("")]
        public string UserName { get; set; } = string.Empty;

        [Display(Name = "Patronymic")]
        [DefaultValue("")]
        public string? UserPatronymic { get; set; }

        [Required(ErrorMessage = "Не указана почта!")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Не указан пароль!")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "Длина должна быть от 6 до 64 символов")]
        [DefaultValue("12345678")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Не указан повторяющийся пароль!")]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "Длина должна быть от 6 до 64 символов")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DefaultValue("12345678")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public RoleDTO Role { get; set; } = new();
    }
}
