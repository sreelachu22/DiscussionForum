using DiscussionForum.UnitOfWork;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscussionForum.Services
{
    public class CommunityCategoryService : ICommunityCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public CommunityCategoryService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<CommunityCategory>> GetCommunityCategoriesAsync()
        {
            return await Task.FromResult(_unitOfWork.CommunityCategory.GetAll());
        }

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
