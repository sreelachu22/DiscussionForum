using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.ComponentModel.DataAnnotations.Schema;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Seeds;

namespace DiscussionForum.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Define DbSet for each entity
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<CommunityStatus> CommunityStatus { get; set; }
        public DbSet<CommunityCategory> CommunityCategories { get; set; }
        public DbSet<ThreadStatus> ThreadStatus { get; set; }
        public DbSet<User> Users{ get; set; }

        /*public DbSet<UserLog> UserLog { get; set; }*/
        public DbSet<Threads> Threads { get; set; }
        public DbSet<ThreadVote> ThreadVotes { get; set; }
        public DbSet<ReplyVote> ReplyVotes { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<CommunityCategoryMapping> CommunityCategoryMapping { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ThreadTagsMapping> ThreadTagsMapping { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => {
                entity.ToTable(name:"Users");
                // Specify your custom table name here
                entity.Property(e => e.Id).HasColumnName("UserId");
                // Specify your custom column name here
            });
            // Configure relationships and delete behaviors
            modelBuilder.Entity<User>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Designation)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DesignationID)
                .OnDelete(DeleteBehavior.Restrict);


            // Relationships for Threads entity
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

            // Relationships for ThreadVote entity
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

            // Relationships for UserRoleMapping entity
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

            // Relationships for User entity
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

            // Relationships for CommunityCategoryMapping entity
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

            // Relationships for Community entity
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

            modelBuilder.Entity<Reply>()
                .HasOne(t => t.CreatedByUser)
                .WithOne()
                .HasForeignKey<Reply>(u => u.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reply>()
                .HasOne(t => t.ModifiedByUser)
                .WithOne()
                .HasForeignKey<Reply>(u => u.ModifiedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reply>()
                .HasMany(t => t.ReplyVotes)
                .WithOne(tv => tv.Reply)
                .HasForeignKey(tv => tv.ReplyID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reply>()
                .HasOne(r => r.Threads)
                .WithMany() // No navigation property on the other side
                .HasForeignKey(r => r.ThreadID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reply>()
               .HasOne(r => r.ParentReply)
               .WithMany()  // Remove the argument to indicate that a reply can have only one parent
               .HasForeignKey(r => r.ParentReplyID)
               .OnDelete(DeleteBehavior.Restrict);

            //Relationships for Reply entity
            modelBuilder.Entity<Reply>()
            .HasOne(r => r.CreatedByUser)
            .WithMany()
            .HasForeignKey(r => r.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reply>()
                .HasOne(r => r.ModifiedByUser)
                .WithMany()
                .HasForeignKey(r => r.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            //Relationships for ReplyVote entity
            modelBuilder.Entity<ReplyVote>()
                .HasOne(rv => rv.CreatedByUser)
                .WithMany(rv => rv.ReplyVotesCreatedBy)
                .HasForeignKey(rv => rv.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReplyVote>()
                .HasOne(rv => rv.ModifiedByUser)
                .WithMany(u => u.ReplyVotesModifiedBy)
                .HasForeignKey(rv => rv.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.ApplyConfiguration(new UserSeed());
            UserRoleMappingSeed.SeedUserRoleMapping(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
