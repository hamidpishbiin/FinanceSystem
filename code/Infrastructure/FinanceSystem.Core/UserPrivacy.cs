namespace FinanceSystem.Core
{
    public class UserPrivacy
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public long Status { get; set; }
        public List<int> RolesList { get; set; }
        public List<long> AllowPermissions { get; set; }
    }
}
