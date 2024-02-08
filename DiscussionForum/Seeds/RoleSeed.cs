/*using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DiscussionForum.Enum;

namespace InvoiceTrack.Infrastructure.Seeds
{

    public class RoleSeed : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            // Seed default user roles
            builder.HasData(
                new IdentityRole { Id = ((int)UserRole.SuperAdmin).ToString(), Name = UserRole.SuperAdmin.ToString(), NormalizedName = UserRole.SuperAdmin.ToString().ToUpper(), ConcurrencyStamp = "9923bbc4-e49c-489b-84bc-7978f6b05817" },
                new IdentityRole { Id = ((int)UserRole.CommunityHead).ToString(), Name = UserRole.CommunityHead.ToString(), NormalizedName = UserRole.CommunityHead.ToString().ToUpper(), ConcurrencyStamp = "6a60704f-2a17-4e35-90a8-3323a8c09f42" },
                new IdentityRole { Id = ((int)UserRole.User).ToString(), Name = UserRole.User.ToString(), NormalizedName = UserRole.User.ToString().ToUpper(), ConcurrencyStamp = "cd18ec13-457a-4f6c-838b-fe33df1fb517" }
            );
        }
    }
}*/