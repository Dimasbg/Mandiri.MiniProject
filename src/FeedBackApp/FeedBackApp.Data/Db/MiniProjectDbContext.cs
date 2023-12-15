using FeedBackApp.Data.Dao;
using Microsoft.EntityFrameworkCore;

namespace FeedBackApp.Data.Db
{
    public class MiniProjectDbContext : DbContext
    {
        public MiniProjectDbContext(DbContextOptions<MiniProjectDbContext> options) : base(options)
        {
        }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}
