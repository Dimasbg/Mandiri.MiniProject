using Microsoft.EntityFrameworkCore;
using TicketingApp.Data.Dao;

namespace TicketingApp.Data.Db
{
    public class MiniProjectDbContext : DbContext
    {
        public MiniProjectDbContext(DbContextOptions<MiniProjectDbContext> options) : base(options)
        {
        }

        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
