using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
using DiscussionForum.UnitOfWork;
using Moq;
using Xunit;

namespace DiscussionForum.Tests.Services
{
    public class ThreadStatusServiceTests
    {
        [Fact]
        public async Task GetThreadStatusAsync_ReturnsThreadStatusCollection()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var expectedThreadStatuses = new List<ThreadStatus>
            {
                new ThreadStatus { ThreadStatusID = 1, ThreadStatusName = "Close" },
                new ThreadStatus { ThreadStatusID = 2, ThreadStatusName = "Open" }
            };
            unitOfWorkMock.Setup(uow => uow.ThreadStatus.GetAll()).Returns(expectedThreadStatuses);
            var threadStatusService = new ThreadStatusService(unitOfWorkMock.Object);

            // Act
            var threadStatuses = await threadStatusService.GetThreadStatusAsync();

            // Assert
            Assert.NotNull(threadStatuses);
            Assert.Equal(expectedThreadStatuses, threadStatuses);
        }

        [Fact]
        public async Task GetThreadStatusByIdAsync_ReturnsThreadStatus()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var expectedThreadStatus = new ThreadStatus { ThreadStatusID = 1, ThreadStatusName = "Close" };
            unitOfWorkMock.Setup(uow => uow.ThreadStatus.GetById(It.IsAny<int>())).Returns(expectedThreadStatus);
            var threadStatusService = new ThreadStatusService(unitOfWorkMock.Object);

            // Act
            var threadStatus = await threadStatusService.GetThreadStatusByIdAsync(1);

            // Assert
            Assert.NotNull(threadStatus);
            Assert.Equal(expectedThreadStatus, threadStatus);
        }

    }
}
