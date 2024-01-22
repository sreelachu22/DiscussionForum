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

        public DbSet<User> Users { get; set; }

        public DbSet<Threads> Threads { get; set; }

        public DbSet<Community> Communities { get; set; }

        public DbSet<CommunityCategoryMapping> CommunityCategoryMapping {  get; set; }
        
        public DbSet<Notice> Notices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure CreatedByUser relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.CreatedByUser)
                .WithOne()
                .HasForeignKey<User>(u => u.CreatedBy) // Configure ModifiedByUser relationship
                .IsRequired(false)  // Assuming CreatedBy can be null
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.ModifiedByUser)
                .WithOne()
                .HasForeignKey<User>(u => u.ModifiedBy)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

          
            modelBuilder.Entity<CommunityCategoryMapping>()
                .HasOne(ccm => ccm.CreatedByUser)
                .WithMany()
                .HasForeignKey(ccm => ccm.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CommunityCategoryMapping>()
                .HasOne(ccm => ccm.ModifiedByUser)
                .WithMany()
                .HasForeignKey(ccm => ccm.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }

}

