namespace UserManagementBack.Models
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; } //Admin, Manager, User
    }
}
