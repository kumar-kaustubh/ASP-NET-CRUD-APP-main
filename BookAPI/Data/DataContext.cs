using Microsoft.EntityFrameworkCore;
using BookAPI.Models;

namespace BookAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<BookAPIDb> Books { get; set; }
    }
}
