namespace UserManagementBack.Data
{
    public interface IDataManager
    {
        public IUserRepository Users { get; }
    }
}
