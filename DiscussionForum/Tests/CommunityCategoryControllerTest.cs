using DiscussionForum.Controllers;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DiscussionForum.Tests.Controllers
{
    public class CommunityCategoryControllerTest
    {
        /// <summary>
        /// Verifies that the GetCommunityCategories method returns an OkResult with an empty list of community categories.
        /// </summary>
        [Fact]
        public async Task GetCommunityCategories_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<ICommunityCategoryService>();
            mockService.Setup(service => service.GetCommunityCategoriesAsync()).ReturnsAsync(new List<CommunityCategory>());

            var controller = new CommunityCategoryController(mockService.Object);

            // Act
            var result = await controller.GetCommunityCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var categories = Assert.IsAssignableFrom<List<CommunityCategory>>(okResult.Value);
            Assert.Empty(categories);
        }

        /// <summary>
        /// Verifies that the GetCommunityCategoryById method returns an OkResult with the correct community category when provided with a valid ID.
        /// </summary>
        [Fact]
        public async Task GetCommunityCategoryById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<ICommunityCategoryService>();
            var categoryId = 1;
            var category = new CommunityCategory { CommunityCategoryID = categoryId, CommunityCategoryName = "Category 1" };
            mockService.Setup(service => service.GetCommunityCategoryByIdAsync(categoryId)).ReturnsAsync(category);

            var controller = new CommunityCategoryController(mockService.Object);

            // Act
            var result = await controller.GetCommunityCategoryById(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var retrievedCategory = Assert.IsType<CommunityCategory>(okResult.Value);
            Assert.Equal(categoryId, retrievedCategory.CommunityCategoryID);
        }

        /// <summary>
        /// Verifies that the GetCommunityCategoryById method returns a NotFoundResult when provided with an invalid ID.
        /// </summary>
        [Fact]
        public async Task GetCommunityCategoryById_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var mockService = new Mock<ICommunityCategoryService>();
            var invalidCategoryId = 999;
            mockService.Setup(service => service.GetCommunityCategoryByIdAsync(invalidCategoryId)).ReturnsAsync((CommunityCategory)null);

            var controller = new CommunityCategoryController(mockService.Object);

            // Act
            var result = await controller.GetCommunityCategoryById(invalidCategoryId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
