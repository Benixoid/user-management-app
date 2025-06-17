namespace UserManagementBack.Models.DTO
{
    public class UserDTO : BaseDTO
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
    }
}
