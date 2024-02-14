using DiscussionForum.Controllers;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DiscussionForum.Tests.Controllers
{
    public class CommunityCategoryControllerTest
    {
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

        [Fact]
        public async Task GetCommunityCategoryById_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var mockService = new Mock<ICommunityCategoryService>();
            var invalidCategoryId = 999; // An ID that doesn't exist
            mockService.Setup(service => service.GetCommunityCategoryByIdAsync(invalidCategoryId)).ReturnsAsync((CommunityCategory)null);

            var controller = new CommunityCategoryController(mockService.Object);

            // Act
            var result = await controller.GetCommunityCategoryById(invalidCategoryId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
