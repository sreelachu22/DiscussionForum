using DiscussionForum.Services;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using DiscussionForum.Data;

namespace DiscussionForum.Tests.Services
{
    public class CommunityCategoryServiceTest
    {
        /*[Fact]
        public async Task GetCommunityCategoriesAsync_ReturnsCorrectResult()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockContext = new Mock<AppDbContext>();
            var service = new CommunityCategoryService(mockUnitOfWork.Object, mockContext.Object);

            var categories = new List<CommunityCategory>
            {
                new CommunityCategory { CommunityCategoryID = 1, CommunityCategoryName = "Category 1", IsDeleted = false },
                new CommunityCategory { CommunityCategoryID = 2, CommunityCategoryName = "Category 2", IsDeleted = false }
            };

            mockUnitOfWork.Setup(uow => uow.CommunityCategory.GetAll())
                          .Returns(categories.AsQueryable());

            // Act
            var result = await service.GetCommunityCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categories.Count, result.Count());
            foreach (var category in result)
            {
                Assert.False(category.IsDeleted);
            }
        }*/

        /*[Fact]
        public async Task GetCommunityCategoryByIdAsync_ReturnsCorrectResult()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockContext = new Mock<AppDbContext>();
            var service = new CommunityCategoryService(mockUnitOfWork.Object, mockContext.Object);

            var categoryId = 1;
            var category = new CommunityCategory { CommunityCategoryID = categoryId, CommunityCategoryName = "Category 1", IsDeleted = false };

            // Set up mock to return the category when Find method is called
            mockContext.Setup(context => context.CommunityCategories.Find(categoryId))
                       .Returns(category);

            // Act
            var result = await service.GetCommunityCategoryByIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.CommunityCategoryID, result.CommunityCategoryID);
            Assert.Equal(category.CommunityCategoryName, result.CommunityCategoryName);
            Assert.False(result.IsDeleted);
        }*/


        /*[Fact]
        public async Task GetCommunityCategoryByIdAsync_ReturnsCorrectResult()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockContext = new Mock<AppDbContext>();
            var service = new CommunityCategoryService(mockUnitOfWork.Object, mockContext.Object);

            var categoryId = 1;
            var category = new CommunityCategory { CommunityCategoryID = categoryId, CommunityCategoryName = "Category 1", IsDeleted = false };

            mockContext.Setup(context => context.CommunityCategories.Find(categoryId))
                       .Returns(category);

            // Act
            var result = await service.GetCommunityCategoryByIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.CommunityCategoryID, result.CommunityCategoryID);
            Assert.Equal(category.CommunityCategoryName, result.CommunityCategoryName);
            Assert.False(result.IsDeleted);
        }*/

        /* [Fact]
         public async Task GetCommunityCategoryByIdAsync_ReturnsIncorrectResult()
         {
             // Arrange
             var mockUnitOfWork = new Mock<IUnitOfWork>();
             var mockContext = new Mock<AppDbContext>();
             var service = new CommunityCategoryService(mockUnitOfWork.Object, mockContext.Object);

             var categoryId = 1;

             // Set up mock to return null when Find method is called
             mockContext.Setup(context => context.CommunityCategories.Find(categoryId))
                        .Returns((CommunityCategory)null);

             // Act
             var result = await service.GetCommunityCategoryByIdAsync(categoryId);

             // Assert
             Assert.Null(result); // Expecting null result
         }*/


    }
}
