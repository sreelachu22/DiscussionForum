.using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        
        public DbSet<Role> Roles { get; set; }

        public DbSet<ForumStatus> ForumStatus { get; set; }

        public DbSet<ForumCategory> ForumCategories { get; set; }
    }

}

