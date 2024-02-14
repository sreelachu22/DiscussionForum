using DiscussionForum.UnitOfWork;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Services
{
    public class CommunityCategoryService : ICommunityCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public CommunityCategoryService(IUnitOfWork unitOfWork,AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }


        /// <summary>
        /// Retrieves all community categories asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing the collection of community categories.</returns>
        public async Task<IEnumerable<CommunityCategory>> GetCommunityCategoriesAsync()
        {
            try
            {
                var communityCategories = _unitOfWork.CommunityCategory
            .GetAll()
            .Where(cc => !cc.IsDeleted)
            .ToList();

                return await Task.FromResult(communityCategories);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in GetCommunityCategoriesAsync: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves a community category by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the community category to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation containing the retrieved community category.</returns>
        public async Task<CommunityCategory> GetCommunityCategoryByIdAsync(long id)
        {
            try
            {
                return await Task.FromResult(_context.CommunityCategories.Find(id));
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching community categories.", ex);
            }
        }

        /// <summary>
        /// Creates a new community category asynchronously.
        /// </summary>
        /// <param name="communityCategoryName">The name of the community category to create.</param>
        /// <returns>A task that represents the asynchronous operation containing the created community category.</returns>
        public async Task<CommunityCategory> CreateCommunityCategoryAsync(string communityCategoryName)
        {
            try
            {
                return await Task.FromResult(CreateCategory(communityCategoryName));
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while creating community category.", ex);
            }
        }

        private CommunityCategory CreateCategory(string communityCategoryName)
        {
            CommunityCategory communityCategory = new CommunityCategory { CommunityCategoryName = communityCategoryName, IsDeleted = false };
            _unitOfWork.CommunityCategory.Add(communityCategory);
            _unitOfWork.Complete();
            return communityCategory;
        }

        /// <summary>
        /// Updates a community category asynchronously.
        /// </summary>
        /// <param name="id">The ID of the community category to update.</param>
        /// <param name="communityCategoryDto">The community category data to update.</param>
        /// <returns>A task that represents the asynchronous operation containing the updated community category.</returns>
        public async Task<CommunityCategory> UpdateCommunityCategoryAsync(long id, CommunityCategory communityCategoryDto)
        {
            try
            {
                var existingCommunityCategory = _context.CommunityCategories.Find(id);

                if (existingCommunityCategory != null)
                {
                    existingCommunityCategory.CommunityCategoryName = communityCategoryDto.CommunityCategoryName;
                    existingCommunityCategory.IsDeleted = communityCategoryDto.IsDeleted;
                    await _context.SaveChangesAsync();
                    return existingCommunityCategory;
                }
                else
                {
                    throw new Exception($"Community category with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while updating community category with ID {id}.", ex);
            }
        }

        /// <summary>
        /// Deletes a community category asynchronously.
        /// </summary>
        /// <param name="communityCategoryId">The ID of the community category to delete.</param>
        /// <returns>A task that represents the asynchronous operation containing the deleted community category.</returns>
        public async Task<CommunityCategory> DeleteCommunityCategoryAsync(long communityCategoryId)
        {
            try
            {
                var deletedCategory = await Task.Run(() => DeleteCategory(communityCategoryId));
                return deletedCategory;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while deleting community category with ID {communityCategoryId}.", ex);
            }
        }

        /// <summary>
        /// Soft delete - update the isDeleted to true.
        /// </summary>
        /// <param name="communityCategoryId"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        private CommunityCategory DeleteCategory(long communityCategoryId)
        {
            var communityCategory = _context.CommunityCategories.Find(communityCategoryId);

            if (communityCategory != null)
            {
                communityCategory.IsDeleted = true;
                _context.SaveChanges();

                return communityCategory; // Return the deleted category
            }
            else
            {
                throw new ApplicationException($"Community category with ID {communityCategoryId} not found.");
            }
        }
    }
}
