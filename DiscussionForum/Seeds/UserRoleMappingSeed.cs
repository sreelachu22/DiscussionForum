/*using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Seeds
{
    public class UserRoleSeed : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.ToTable("UserRoleMapping"); // Set the table name explicitly

            builder.HasKey(ur => new { ur.UserId, ur.RoleId }); // Define composite primary key

            builder.Property(ur => ur.UserRoleMappingID)
                .HasColumnName("UserRoleMappingID"); // Map UserRoleMappingID to the corresponding column in UserRoleMapping table

            builder.Property(ur => ur.UserId)
                .HasColumnName("UserID"); // Map UserId to the corresponding column in UserRoleMapping table

            builder.Property(ur => ur.RoleId)
                .HasColumnName("RoleID"); // Map RoleId to the corresponding column in UserRoleMapping table
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",
                    UserId = "c630e1e2-24c7-4fd0-bf53-a95597bcbca9"
                }
            );
        }
    }
}
*/

using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace DiscussionForum.Seeds
{
    public class UserRoleMappingSeed
    {
        public static void SeedUserRoleMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRoleMapping>().HasData(
                new UserRoleMapping
                {
                    UserRoleMappingID = 1, // Assuming UserRoleMappingID is an integer and you are seeding the first entry
                    UserID = Guid.Parse("c630e1e2-24c7-4fd0-bf53-a95597bcbca9"),
                    RoleID = 1,
                    IsDeleted = false,
                    CreatedBy = Guid.Parse("c630e1e2-24c7-4fd0-bf53-a95597bcbca9"), // You should provide a valid value for CreatedBy and ModifiedBy
                    CreatedAt = DateTime.UtcNow,
                    ModifiedBy = null,
                    ModifiedAt = null
                }
            );
        }
    }
}
