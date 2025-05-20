using System.ComponentModel.DataAnnotations;

namespace EduFlowApi.DTOs.AuthDTO
{
    public class DeleteAccountDTO
    {
        [Required(ErrorMessage = "Id удаляемого пользователя не указан!")]
        [Display(Name = "UserId")]
        public Guid UserId { get; set; }
    }
}
