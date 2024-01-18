using DiscussionForum.UnitOfWork;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscussionForum.Services
{
    public class ForumCategoryService : IForumCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public ForumCategoryService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<ForumCategory>> GetForumCategoriesAsync()
        {
            return await Task.FromResult(_unitOfWork.ForumCategory.GetAll());
        }

        public async Task<ForumCategory> GetForumCategoryByIdAsync(long id)
        {
            try
            {
                return await Task.FromResult(_context.ForumCategories.Find(id));
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching forum categories.", ex);
            }
        }

        public async Task<ForumCategory> CreateForumCategoryAsync(string forumCategoryName)
        {
            try
            {
                return await Task.FromResult(CreateCategory(forumCategoryName));
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while creating forum category.", ex);
            }
        }

        private ForumCategory CreateCategory(string forumCategoryName)
        {
            ForumCategory forumCategory = new ForumCategory { ForumCategoryName = forumCategoryName, IsDeleted = false };
            _unitOfWork.ForumCategory.Add(forumCategory);
            _unitOfWork.Complete();
            return forumCategory;
        }

        public async Task<ForumCategory> UpdateForumCategoryAsync(long id, ForumCategory forumCategoryDto)
        {
            try
            {
                var existingForumCategory = _context.ForumCategories.Find(id);

                if (existingForumCategory != null)
                {
                    existingForumCategory.ForumCategoryName = forumCategoryDto.ForumCategoryName;
                    existingForumCategory.IsDeleted = forumCategoryDto.IsDeleted;
                    await _context.SaveChangesAsync();
                    return existingForumCategory;
                }
                else
                {
                    throw new Exception($"Forum category with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while updating forum category with ID {id}.", ex);
            }
        }

        public async Task<ForumCategory> DeleteForumCategoryAsync(long forumCategoryId)
        {
            try
            {
                var deletedCategory = await Task.Run(() => DeleteCategory(forumCategoryId));
                return deletedCategory;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while deleting forum category with ID {forumCategoryId}.", ex);
            }
        }

        private ForumCategory DeleteCategory(long forumCategoryId)
        {
            var forumCategory = _context.ForumCategories.Find(forumCategoryId);

            if (forumCategory != null)
            {
                forumCategory.IsDeleted = true;
                _context.SaveChanges();
                return forumCategory; // Return the deleted category
            }
            else
            {
                throw new ApplicationException($"Forum category with ID {forumCategoryId} not found.");
            }
        }
    }
}
