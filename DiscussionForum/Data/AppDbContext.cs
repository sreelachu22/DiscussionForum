using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.ComponentModel.DataAnnotations.Schema;
namespace DiscussionForum.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Designation> Designations { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<CommunityStatus> CommunityStatus { get; set; }

        public DbSet<CommunityCategory> CommunityCategories { get; set; }

        public DbSet<ThreadStatus> ThreadStatus { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Threads> Threads { get; set; }

        public DbSet<ThreadVote> ThreadVotes { get; set; }

        public DbSet<Community> Communities { get; set; }

        public DbSet<CommunityCategoryMapping> CommunityCategoryMapping { get; set; }

        public DbSet<Notice> Notices { get; set; }

        public DbSet<Reply> Replies { get; set; }

        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<ThreadTagsMapping> ThreadTagsMapping { get; set; }

        public DbSet<ThreadVote> ThreadVotes { get; set; }

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

            modelBuilder.Entity<Threads>()
                .HasMany(t => t.ThreadVotes) 
                .WithOne(tv => tv.Thread)
                .HasForeignKey(tv => tv.ThreadID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ThreadVote>()
                .HasOne(tv => tv.CreatedByUser)
                .WithMany(tv => tv.ThreadVotesCreatedBy)
                .HasForeignKey(tv => tv.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ThreadVote>()
                .HasOne(tv => tv.ModifiedByUser)
                .WithMany(tv => tv.ThreadVotesModifiedBy)
                .HasForeignKey(tv => tv.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);





            modelBuilder.Entity<UserRoleMapping>()
                .HasOne(ur => ur.CreatedByUser)
                .WithMany()
                .HasForeignKey(ur => ur.CreatedBy)  
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRoleMapping>()
                .HasOne(ur => ur.ModifiedByUser)
                .WithMany()
                .HasForeignKey(ur => ur.ModifiedBy)  
                .OnDelete(DeleteBehavior.Restrict);




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

            modelBuilder.Entity<Community>()
                .HasOne(c => c.CreatedByUser)
                .WithMany()
                .HasForeignKey(c => c.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Community>()
                .HasOne(c => c.ModifiedByUser)
                .WithMany()
                .HasForeignKey(c => c.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }

}

