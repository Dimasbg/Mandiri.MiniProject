namespace UserAuthApp.Data.Dao
{
    public class Role
    {
        public int RoleId { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<UserRole> UserRoles{ get; set; }

        public void Update(Role d)
        {
            Name = d.Name;
        }
    }
}
