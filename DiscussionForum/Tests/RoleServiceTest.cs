using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
using DiscussionForum.UnitOfWork;
using Moq;
using Xunit;

namespace DiscussionForum.Tests.Services
{
    public class RoleServiceTests
    {
        /// <summary>
        /// Verifies that the GetAllRoles method returns a collection of roles without the SuperAdmin role.
        /// </summary>
        [Fact]
        public async Task GetAllRoles_Valid_ReturnsRolesCollectionWithoutSuperAdminRole()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var roles = new List<Role>
            {
                new Role { RoleID = 1, RoleName = "SuperAdmin" },
                new Role { RoleID = 2, RoleName = "CommunityHead" },
                new Role { RoleID = 3, RoleName = "User" }
            };
            unitOfWorkMock.Setup(uow => uow.Role.GetAll()).Returns(roles.AsQueryable());

            var roleService = new RoleService(unitOfWorkMock.Object);

            // Act
            var result = await roleService.GetAllRoles();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); // Expecting 2 roles without SuperAdmin
            Assert.DoesNotContain(result, r => r.RoleID == 1); // Ensure SuperAdmin role is filtered out
        }

        /// <summary>
        /// Verifies that the GetAllRoles method returns an empty roles collection when there are no roles available.
        /// </summary>
        [Fact]
        public async Task GetAllRoles_Valid_ReturnsEmptyRolesCollection()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var roles = new List<Role>(); // Empty roles collection
            unitOfWorkMock.Setup(uow => uow.Role.GetAll()).Returns(roles.AsQueryable());

            var roleService = new RoleService(unitOfWorkMock.Object);

            // Act
            var result = await roleService.GetAllRoles();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
