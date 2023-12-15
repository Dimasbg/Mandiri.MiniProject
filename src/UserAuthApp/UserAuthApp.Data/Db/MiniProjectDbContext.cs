using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using UserAuthApp.Data.Dao;

namespace UserAuthApp.Data.Db
{
    public class MiniProjectDbContext : DbContext
    {
        public MiniProjectDbContext(DbContextOptions<MiniProjectDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(bc => new { bc.UserId, bc.RoleId});

            modelBuilder.Entity<UserRole>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserRoles)
                .HasForeignKey(bc => bc.UserId);


            modelBuilder.Entity<UserRole>()
                .HasOne(bc => bc.Role)
                .WithMany(b => b.UserRoles)
                .HasForeignKey(bc => bc.RoleId);
        }
    }
}
