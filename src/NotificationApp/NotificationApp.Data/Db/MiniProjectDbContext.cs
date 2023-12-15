using Microsoft.EntityFrameworkCore;
using NotificationApp.Data.Dao;

namespace NotificationApp.Data.Db
{
    public class MiniProjectDbContext : DbContext
    {
        public MiniProjectDbContext(DbContextOptions<MiniProjectDbContext> options) : base(options)
        {
        }
        public DbSet<Notification> Notifications { get; set; }
    }
}
