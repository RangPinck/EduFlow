namespace EduFlowApi.DTOs.UserDTOs
{
    public class SubscribeUserDTO
    {
        public Guid UserId { get; set; }

        public string UserSurname { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string? UserPatronymic { get; set; }

        public string UserRole { get; set; } = string.Empty;
    }
}
