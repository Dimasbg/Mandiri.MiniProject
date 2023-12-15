namespace UserAuthApp.Data.Dao
{
    public class User
    {

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        public void Update(User d)
        {
            UserName = d.UserName;
            Email = d.Email;
        }
    }
}
