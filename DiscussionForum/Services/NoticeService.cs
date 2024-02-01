using DiscussionForum.UnitOfWork;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DiscussionForum.Services
{
    public class NoticeService : INoticeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        // NoticeService: A service class for handling CRUD operations related to notices.

        // Constructor: Initializes the NoticeService with the required dependencies.
        // - _unitOfWork: Unit of work for managing database transactions.
        // - _context: DbContext for database operations.

        public NoticeService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        // GetNoticesAsync: Retrieves active notices asynchronously, filtering out deleted notices and those with expired dates.
        // - Returns: A collection of active notices.
        public async Task<IEnumerable<Notice>> GetNoticesAsync()
        {
            DateTime currentDate = DateTime.Now;

            return await Task.FromResult(_unitOfWork.Notice.GetAll(notice =>
                !notice.IsDeleted &&
                (notice.ExpiresAt == null || notice.ExpiresAt > currentDate)
            ));
        }

        // CreateNoticeAsync: Creates a new notice asynchronously.
        // - Parameters: Community ID, title, content, expiration date, creator ID.
        // - Returns: The created notice.
        public async Task<Notice> CreateNoticeAsync(int communityId, string title, string content, DateTime? expiresAt, Guid createdBy)
        {
            try
            {
                return await Task.FromResult(CreateNotice(communityId, title, content, expiresAt, createdBy));
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while creating notice.", ex);
            }
        }

        // CreateNotice: Helper method to create a notice synchronously.
        // - Parameters: Community ID, title, content, expiration date, creator ID.
        // - Returns: The created notice.
        private Notice CreateNotice(int communityId, string title, string content, DateTime? expiresAt, Guid createdBy)
        {
            Notice notice = new Notice
            {
                CommunityID = communityId,
                Title = title,
                Content = content,
                ExpiresAt = expiresAt,
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            _unitOfWork.Notice.Add(notice);
            _unitOfWork.Complete();
            return notice;
        }

        // UpdateNoticeAsync: Updates an existing notice asynchronously.
        // - Parameters: Notice ID, community ID, title, content, expiration date, modifier ID.
        // - Returns: The updated notice.
        public async Task<Notice> UpdateNoticeAsync(int id, int communityId, string title, string content, DateTime? expiresAt, Guid modifiedBy)
        {
            try
            {
                return await Task.FromResult(UpdateNotice(id, communityId, title, content, expiresAt, modifiedBy));
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating notice.", ex);
            }
        }

        // UpdateNotice: Helper method to update a notice synchronously.
        // - Parameters: Notice ID, community ID, title, content, expiration date, modifier ID.
        // - Sets isDeleted as false and the current datetime as ModifiedAt
        // Returns: The updated notice.
        private Notice UpdateNotice(int id, int communityId, string title, string content, DateTime? expiresAt, Guid modifiedBy)
        {
            var existingNotice = _context.Notices.Find(id);

            if (existingNotice != null)
            {
                existingNotice.CommunityID = communityId;
                existingNotice.Title = title;
                existingNotice.Content = content;
                existingNotice.ExpiresAt = expiresAt;
                existingNotice.IsDeleted = false;
                existingNotice.ModifiedBy = modifiedBy;
                existingNotice.ModifiedAt = DateTime.UtcNow;

                _context.SaveChanges(); // Use SaveChanges instead of SaveChangesAsync since it's not awaited

                return existingNotice;
            }
            else
            {
                throw new Exception($"Notice with ID {id} not found.");
            }
        }

        // DeleteNoticeAsync: Deletes a notice asynchronously by marking it as deleted.
        // - Parameters: Notice ID.
        // - Returns: The deleted notice.
        public async Task<Notice> DeleteNoticeAsync(int id)
        {
            try
            {
                var deletedNotice = await Task.Run(() => DeleteNotice(id));
                return deletedNotice;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while deleting notice with ID {id}.", ex);
            }
        }

        // DeleteNotice: Helper method to delete a notice synchronously by marking it as deleted.
        // - Parameters: Notice ID.
        // - Returns: The deleted notice.
        private Notice DeleteNotice(int id)
        {
            var notice = _context.Notices.Find(id);

            if (notice != null)
            {
                notice.IsDeleted = true;
                _context.SaveChanges();
                return notice; // Return the deleted notice
            }
            else
            {
                throw new ApplicationException($"Notice with ID {id} not found.");
            }
        }
    }
}
