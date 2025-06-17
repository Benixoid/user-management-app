namespace UserManagementBack.Models
{
    public class User : BaseEntity
    {
        private string _role;
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public string Role
        {
            get => _role;
            set
            {
                if (value != "Admin" && value != "Manager" && value != "User")
                {
                    throw new ArgumentException("Role must be either 'Admin', 'Manager', or 'User'.");
                }
                _role = value;
            }
        }
    }
}
