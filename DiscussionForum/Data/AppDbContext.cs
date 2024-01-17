using Microsoft.EntityFrameworkCore;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ThreadStatus> ThreadStatus { get; set; }
    }
}
