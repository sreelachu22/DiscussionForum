using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

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

        public DbSet<Notice> Notices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure CreatedByUser relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.CreatedByUser)
                .WithOne()
                .HasForeignKey<User>(u => u.CreatedBy)
                .IsRequired(false)  // Assuming CreatedBy can be null
                .OnDelete(DeleteBehavior.Restrict);

            // Configure ModifiedByUser relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.ModifiedByUser)
                .WithOne()
                .HasForeignKey<User>(u => u.ModifiedBy)
                .IsRequired(false)  // Assuming ModifiedBy can be null
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }


    }

}

