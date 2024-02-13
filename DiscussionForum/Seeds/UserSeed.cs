/*using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Seeds
{
    public class UserSeed : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Seed default users with roles
            builder.HasData(new User
            {
                Id = Guid.Parse("c630e1e2-24c7-4fd0-bf53-a95597bcbca9"),
                UserName = "sreelakshmi.pm.@experionglobal.com",
                NormalizedUserName = "SREELAKSHMI.PM@EXPERIONGLOBAL.COM",
                Name = "Sreelakshmi P M",
                Email = "sreelakshmi.pm.@experionglobal.com",
                NormalizedEmail = "SREELAKSHMI.PM@EXPERIONGLOBAL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEEX7lDhpbnySiG8ZXrgH9tZrWrlnV3ieENPAQ3fR3qntxFCTu/v+OsAQlJiJTlKREA==",
                SecurityStamp = string.Empty,
                ConcurrencyStamp = "c430e2e2-24c7-4fd0-bf43-a95597ccbca9",
                DepartmentID = 1,
                DesignationID = 1,
                IsDeleted = false,
                CreatedBy = null,
                CreatedAt = DateTime.Now,
                ModifiedBy = null,
                ModifiedAt = null
            });
        }
    }
}
*/