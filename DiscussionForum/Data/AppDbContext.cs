using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<User> Users { get; set; }
        public DbSet<Threads> Threads { get; set; }
        public DbSet<ThreadVote> ThreadVotes { get; set; }
        public DbSet<ReplyVote> ReplyVotes { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<CommunityCategoryMapping> CommunityCategoryMapping { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ThreadTagsMapping> ThreadTagsMapping { get; set; }
        public DbSet<UserLog> UserLog { get; set; }
        public DbSet<UserRequestLog> UserRequestLog { get; set; }
        public DbSet<SavedPosts> SavedPosts { get; set; }
        public DbSet<DuplicateThreads> DuplicateThreads { get; set; }
        public DbSet<BestAnswer> BestAnswers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
            //this is a new edit
            modelBuilder.Entity<User>()
                .HasOne(u => u.CreatedByUser)
                .WithOne()
                .HasForeignKey<User>(u => u.CreatedBy)
                .IsRequired(false) // Make the relationship optional
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

            //Relationships and properties for UserLog entity
            modelBuilder.Entity<UserLog>()
                .HasKey(ul => ul.UserLogID);

            modelBuilder.Entity<UserLog>()
                .Property(ul => ul.UserLogID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserLog>()
                .Property(ul => ul.LoginTime)
                .IsRequired();

            modelBuilder.Entity<UserLog>()
                .Property(ul => ul.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<UserLog>()
                .Property(ul => ul.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<UserLog>()
                .HasOne(ul => ul.User)
                .WithMany()
                .HasForeignKey(ul => ul.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserLog>()
                .HasOne(ul => ul.CreatedByUser)
                .WithMany()
                .HasForeignKey(ul => ul.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade); // Corrected from Restrict to Cascade

            modelBuilder.Entity<UserLog>()
                .HasOne(ul => ul.ModifiedByUser)
                .WithMany()
                .HasForeignKey(ul => ul.ModifiedBy)
                .OnDelete(DeleteBehavior.Cascade); // Corrected from Restrict to Cascade

            //Relationships and properties for UserRequestLog entity
            modelBuilder.Entity<UserRequestLog>()
                .HasKey(url => url.UserRequestLogID);

            modelBuilder.Entity<UserRequestLog>()
                .Property(url => url.UserRequestLogID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserRequestLog>()
                .Property(url => url.RequestTime)
                .IsRequired();

            modelBuilder.Entity<UserRequestLog>()
                .Property(url => url.RequestMethod)
                .IsRequired();

            modelBuilder.Entity<UserRequestLog>()
                .Property(url => url.RequestPath)
                .IsRequired();

            modelBuilder.Entity<UserRequestLog>()
                .Property(url => url.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<UserRequestLog>()
                .Property(url => url.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<UserRequestLog>()
                .HasOne(url => url.UserLog)
                .WithMany()
                .HasForeignKey(url => url.UserLogID)
                .OnDelete(DeleteBehavior.Cascade);

            /*modelBuilder.Entity<UserRequestLog>()
                .HasOne(url => url.User)
                .WithMany()
                .HasForeignKey(url => url.UserLog.UserID)
                .OnDelete(DeleteBehavior.Cascade);*/

            modelBuilder.Entity<UserRequestLog>()
                .HasOne(url => url.CreatedByUser)
                .WithMany()
                .HasForeignKey(url => url.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRequestLog>()
                .HasOne(url => url.ModifiedByUser)
                .WithMany()
                .HasForeignKey(url => url.ModifiedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SavedPosts>()
                .HasOne(sp => sp.User)
                .WithMany()
                .HasForeignKey(sp => sp.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SavedPosts>()
                .HasOne(sp => sp.Thread)
                .WithMany()
                .HasForeignKey(sp => sp.ThreadID)
                .OnDelete(DeleteBehavior.Cascade);

            //Relationships and properties for DuplicateThreads entity
            modelBuilder.Entity<DuplicateThreads>()
               .HasKey(dt => dt.DuplicateId);

            modelBuilder.Entity<DuplicateThreads>()
                .Property(dt => dt.DuplicateId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DuplicateThreads>()
                .Property(dt => dt.DuplicateThreadId)
                .IsRequired();

            modelBuilder.Entity<DuplicateThreads>()
                .Property(dt => dt.OriginalThreadId)
                .IsRequired();

            modelBuilder.Entity<DuplicateThreads>()
                .Property(dt => dt.CreatedBy)
                .IsRequired();

            modelBuilder.Entity<DuplicateThreads>()
                .Property(dt => dt.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<DuplicateThreads>()
                .Property(dt => dt.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<DuplicateThreads>()
                .HasOne(dt => dt.DuplicateThread)
                .WithMany()
                .HasForeignKey(dt => dt.DuplicateThreadId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DuplicateThreads>()
                .HasOne(dt => dt.OriginalThread)
                .WithMany()
                .HasForeignKey(dt => dt.OriginalThreadId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DuplicateThreads>()
                .HasOne(dt => dt.CreatedByUser)
                .WithMany()
                .HasForeignKey(dt => dt.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DuplicateThreads>()
                .HasOne(dt => dt.ModifiedByUser)
                .WithMany()
                .HasForeignKey(dt => dt.ModifiedBy)
                .OnDelete(DeleteBehavior.Cascade);

            //Relationships and properties for BestAnswer entity
            modelBuilder.Entity<BestAnswer>()
               .HasKey(ba => ba.BestAnswerId);

            modelBuilder.Entity<BestAnswer>()
                .Property(ba => ba.BestAnswerId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<BestAnswer>()
                .Property(ba => ba.ThreadID)
                .IsRequired();

            modelBuilder.Entity<BestAnswer>()
                .Property(ba => ba.ReplyID)
                .IsRequired();

            modelBuilder.Entity<BestAnswer>()
                .Property(ba => ba.CreatedBy)
                .IsRequired();

            modelBuilder.Entity<BestAnswer>()
                .Property(ba => ba.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<BestAnswer>()
                .Property(ba => ba.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<BestAnswer>()
                .HasOne(ba => ba.Thread)
                .WithMany()
                .HasForeignKey(ba => ba.ThreadID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BestAnswer>()
                .HasOne(ba => ba.Reply)
                .WithMany()
                .HasForeignKey(ba => ba.ReplyID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BestAnswer>()
                .HasOne(ba => ba.CreatedByUser)
                .WithMany()
                .HasForeignKey(ba => ba.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BestAnswer>()
                .HasOne(ba => ba.ModifiedByUser)
                .WithMany()
                .HasForeignKey(ba => ba.ModifiedBy)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
