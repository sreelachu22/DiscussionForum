﻿using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Services
{

    public class CommunityCategoryMappingService : ICommunityCategoryMappingService
    {
        private readonly AppDbContext _context;

        public CommunityCategoryMappingService(AppDbContext context)
        {
            _context = context;
        }

        /*public async Task<CommunityCategoryMapping> CreateCommunityCategoryMappingAsync(CommunityCategoryMappingAPI model)
        {
            var entity = new CommunityCategoryMapping
            {
                CommunityID = model.CommunityID,
                CommunityCategoryID = model.CommunityCategoryID,
                Description = model.Description,
                IsDeleted = false,
                //CreatedBy = model.CreatedBy,
                CreatedAt = DateTime.Now
            };

            _context.CommunityCategoryMapping.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }*/

        public async Task<IEnumerable<CommunityCategoryMappingAPI>> GetAllCategoriesInCommunityAsync(int communityID)
        {
            var result = await (
                from ccm in _context.CommunityCategoryMapping
                join c in _context.Communities on ccm.CommunityID equals c.CommunityID
                join cc in _context.CommunityCategories on ccm.CommunityCategoryID equals cc.CommunityCategoryID
                where ccm.CommunityID == communityID && !ccm.IsDeleted
                select new CommunityCategoryMappingAPI
                {
                    CommunityCategoryMappingID = ccm.CommunityCategoryMappingID,
                    CommunityID = ccm.CommunityID,
                    CommunityCategoryID = ccm.CommunityCategoryID,
                    CommunityCategoryName = cc.CommunityCategoryName,
                    Description = ccm.Description,
                    IsDeleted = ccm.IsDeleted,
                    //CreatedBy = ccm.CreatedBy,
                    CreatedAt = ccm.CreatedAt,
                    //ModifiedBy = ccm.ModifiedBy,
                    ModifiedAt = ccm.ModifiedAt

                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<CommunityCategory>> GetCategoriesNotInCommunityAsync(int communityID)
        {
            var result = await (
                from cc in _context.CommunityCategories
                join ccm in _context.CommunityCategoryMapping
                on cc.CommunityCategoryID equals ccm.CommunityCategoryID into ccMapping
                from mapping in ccMapping.Where(mapping => mapping.CommunityID == communityID && !mapping.IsDeleted).DefaultIfEmpty()
                where mapping == null
                select new CommunityCategory
                {// Set the community ID for the result
                    CommunityCategoryID = cc.CommunityCategoryID,
                    CommunityCategoryName = cc.CommunityCategoryName,
                })
                .ToListAsync();

            return result;
        }


        public async Task<CommunityCategoryMapping> GetCommunityCategoryMappingByIdAsync(int communityCategoryMappingID)
        {
            var result = await (
            from ccm in _context.CommunityCategoryMapping
            join c in _context.Communities on ccm.CommunityID equals c.CommunityID
            join cc in _context.CommunityCategories on ccm.CommunityCategoryID equals cc.CommunityCategoryID
            where ccm.CommunityCategoryMappingID == communityCategoryMappingID && !ccm.IsDeleted
            select new CommunityCategoryMapping
            {
                CommunityCategoryMappingID = ccm.CommunityCategoryMappingID,
                CommunityID = ccm.CommunityID,
                CommunityCategoryID = ccm.CommunityCategoryID,
                Description = ccm.Description,
                IsDeleted = ccm.IsDeleted,
                //CreatedBy = ccm.CreatedBy,
                CreatedAt = ccm.CreatedAt,
                //ModifiedBy = ccm.ModifiedBy,
                ModifiedAt = ccm.ModifiedAt
            }
            ).FirstOrDefaultAsync();

            if (result == null)
                return null;

            return new CommunityCategoryMapping
            {
                CommunityID = result.CommunityID,
                CommunityCategoryID = result.CommunityCategoryID,
                Description = result.Description,
                IsDeleted = result.IsDeleted,
                //CreatedBy = result.CreatedBy,
                CreatedAt = result.CreatedAt,
                //ModifiedBy = result.ModifiedBy,
                ModifiedAt = result.ModifiedAt
            };
        }

        public async Task<CommunityCategoryMapping> CreateCommunityCategoryMappingAsync(int communityID, string communityCategoryName, string description)
        {
            var communityCategory = await _context.CommunityCategories
                .FirstOrDefaultAsync(cc => cc.CommunityCategoryName == communityCategoryName && !cc.IsDeleted);
           
            var entity = new CommunityCategoryMapping
            {

                CommunityID = communityID,
                CommunityCategoryID = communityCategory.CommunityCategoryID,
                Description = description,
                IsDeleted = false,
                CreatedAt = DateTime.Now
            };

            _context.CommunityCategoryMapping.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<CommunityCategoryMapping> UpdateCommunityCategoryMappingAsync(int communityCategoryMappingID, string description)
        {
            var entity = await _context.CommunityCategoryMapping
                .FirstOrDefaultAsync(ccm => ccm.CommunityCategoryMappingID == communityCategoryMappingID && !ccm.IsDeleted);

            /*if (entity == null)
                return false;*/

            entity.Description = description;
            //entity.ModifiedBy = model.ModifiedBy;
            entity.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<CommunityCategoryMapping> DeleteCommunityCategoryMappingAsync(int communityCategoryMappingID)
        {
            var entity = await _context.CommunityCategoryMapping
                .FirstOrDefaultAsync(ccm => ccm.CommunityCategoryMappingID == communityCategoryMappingID && !ccm.IsDeleted);

           /* if (entity == null)
                return false;*/

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return entity;
        }
    }
}