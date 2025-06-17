namespace UserManagementBack.Data
{
    public class DataManager : IDataManager
    {
        public IUserRepository Users { get; }
        public DataManager(IUserRepository userRepository)
        {
            Users = userRepository;
        }
    }    
}
