using EventManagementApp.Data.Dao;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApp.Data.Db
{
    public class MiniProjectDbContext : DbContext
    {
        public MiniProjectDbContext(DbContextOptions<MiniProjectDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }
    }
}
