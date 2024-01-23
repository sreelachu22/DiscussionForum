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

        public NoticeService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<Notice>> GetNoticesAsync()
        {
            return await Task.FromResult(_unitOfWork.Notice.GetAll(notice => !notice.IsDeleted));
        }


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
