using DiscussionForum.Controllers;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DiscussionForum.Tests
{
    public class LoginControllerTest
    {
        [Fact]
        public async Task AdminLogin_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var loginServiceMock = new Mock<ILoginService>();
            loginServiceMock.Setup(x => x.AdminLoginAsync(It.IsAny<AdminLoginDto>()))
                            .ReturnsAsync(new TokenDto()); // Return a valid token

            var controller = new LoginController(loginServiceMock.Object);
            var adminLoginDto = new AdminLoginDto(); // Mock admin login DTO

            // Act
            var result = await controller.AdminLogin(adminLoginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task AdminLogin_InvalidCredentials_ReturnsConflictResult()
        {
            // Arrange
            var loginServiceMock = new Mock<ILoginService>();
            loginServiceMock.Setup(x => x.AdminLoginAsync(It.IsAny<AdminLoginDto>()))
                            .ReturnsAsync((TokenDto)null); // Return null for invalid credentials

            var controller = new LoginController(loginServiceMock.Object);
            var adminLoginDto = new AdminLoginDto(); // Mock admin login DTO

            // Act
            var result = await controller.AdminLogin(adminLoginDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Failed to Log in user.", conflictResult.Value);
        }

        [Fact]
        public async Task ExternalAuthentication_ValidToken_ReturnsOkResult()
        {
            // Arrange
            var loginServiceMock = new Mock<ILoginService>();
            loginServiceMock.Setup(x => x.ExternalAuthenticationAsync(It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync(new TokenDto()); // Return a valid token

            var controller = new LoginController(loginServiceMock.Object);
            var externalAuthDto = new ExternalAuthDto(); // Mock external auth DTO

            // Act
            var result = await controller.ExternalAuthentication(externalAuthDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task ExternalAuthentication_InvalidToken_ReturnsUnauthorizedResult()
        {
            // Arrange
            var loginServiceMock = new Mock<ILoginService>();
            loginServiceMock.Setup(x => x.ExternalAuthenticationAsync(It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync((TokenDto)null); // Return null for invalid token

            var controller = new LoginController(loginServiceMock.Object);
            var externalAuthDto = new ExternalAuthDto(); // Mock external auth DTO

            // Act
            var result = await controller.ExternalAuthentication(externalAuthDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
