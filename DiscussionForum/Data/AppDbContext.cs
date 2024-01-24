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
        public DbSet<Reply> Replies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Threads>()
                .HasOne(t => t.CommunityCategoryMapping)
                .WithOne()
                .HasForeignKey<Threads>(u => u.CommunityCategoryMappingID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Threads>()
                .HasOne(t => t.ThreadStatus)
                .WithOne()
                .HasForeignKey<Threads>(u => u.ThreadStatusID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Threads>()
                .HasOne(t => t.CreatedByUser)
                .WithOne()
                .HasForeignKey<Threads>(u => u.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Threads>()
                .HasOne(t => t.ModifiedByUser)
                .WithOne()
                .HasForeignKey<Threads>(u => u.ModifiedBy)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure CreatedByUser relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.CreatedByUser)
                .WithOne()
                .HasForeignKey<User>(u => u.CreatedBy) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.ModifiedByUser)
                .WithOne()
                .HasForeignKey<User>(u => u.ModifiedBy)
                .OnDelete(DeleteBehavior.Cascade);

          
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

