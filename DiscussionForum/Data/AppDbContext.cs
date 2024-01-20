using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        
        public DbSet<Designation> Designations { get; set; }
        
        public DbSet<Role> Roles { get; set; }

        public DbSet<CommunityStatus> CommunityStatus { get; set; }

        public DbSet<CommunityCategory> CommunityCategories { get; set; }
        
        public DbSet<ThreadStatus> ThreadStatus { get; set; }
    }

}

