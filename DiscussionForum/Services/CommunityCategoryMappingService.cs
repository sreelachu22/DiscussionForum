using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace DiscussionForum.Services
{

    public class CommunityCategoryMappingService : ICommunityCategoryMappingService
    {
        private readonly AppDbContext _context;

        public CommunityCategoryMappingService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(_context));
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


        public async Task<PagedCategoryResult> GetCategories(int communityID, string? term, string? sort, int page, int limit)
        {
            try
            {
                term = string.IsNullOrWhiteSpace(term) ? null : term.Trim().ToLower();

                var query = (
                    from ccm in _context.CommunityCategoryMapping
                    join c in _context.Communities on ccm.CommunityID equals c.CommunityID
                    join cc in _context.CommunityCategories on ccm.CommunityCategoryID equals cc.CommunityCategoryID
                    join t in _context.Threads on ccm.CommunityCategoryMappingID equals t.CommunityCategoryMappingID into threadGroup
                    where ccm.CommunityID == communityID && !ccm.IsDeleted
                    select new CommunityCategoryMappingAPI
                    {
                        CommunityCategoryMappingID = ccm.CommunityCategoryMappingID,
                        CommunityID = ccm.CommunityID,
                        CommunityCategoryID = ccm.CommunityCategoryID,
                        CommunityCategoryName = cc.CommunityCategoryName,
                        Description = ccm.Description,
                        IsDeleted = ccm.IsDeleted,
                        CreatedBy = ccm.CreatedBy,
                        CreatedAt = (DateTime)ccm.CreatedAt,
                        ModifiedAt = ccm.ModifiedAt,
                        // Add the count of threads
                        ThreadCount = threadGroup.Count()
                    }
                );

                // Apply filtering
                if (!string.IsNullOrWhiteSpace(term))
                {
                    query = query.Where(c => c.Description.ToLower().Contains(term) || c.CommunityCategoryName.ToLower().Contains(term));
                }

                // Apply sorting
                if (!string.IsNullOrWhiteSpace(sort))
                {
                    var sortFields = sort.Split(',');
                    var orderByString = string.Join(", ", sortFields.Select(field =>
                    {
                        var trimmedField = field.Trim();
                        var sortOrder = trimmedField.StartsWith("-") ? "descending" : "ascending";
                        var propertyName = trimmedField.TrimStart('-');

                        return $"{propertyName} {sortOrder}";
                    }));

                    query = query.OrderBy(orderByString);
                }
                else
                {
                    query = query.OrderBy(c => c.CommunityCategoryMappingID);
                }

                var totalCount = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalCount / limit);

                var pagedCategories = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

                var pagedCategoryData = new PagedCategoryResult
                {
                    Categories = pagedCategories,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                };

                return pagedCategoryData;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw;
            }
        }



        public async Task<IEnumerable<CommunityCategoryMappingAPI>> GetAllCategoriesInCommunityAsync(int communityID)
        {
            var result = await (
                from ccm in _context.CommunityCategoryMapping
                join c in _context.Communities on ccm.CommunityID equals c.CommunityID
                join cc in _context.CommunityCategories on ccm.CommunityCategoryID equals cc.CommunityCategoryID
                join t in _context.Threads on ccm.CommunityCategoryMappingID equals t.CommunityCategoryMappingID into threadGroup
                where ccm.CommunityID == communityID && !ccm.IsDeleted
                select new CommunityCategoryMappingAPI
                {
                    CommunityCategoryMappingID = ccm.CommunityCategoryMappingID,
                    CommunityID = ccm.CommunityID,
                    CommunityCategoryID = ccm.CommunityCategoryID,
                    CommunityCategoryName = cc.CommunityCategoryName,
                    Description = ccm.Description,
                    IsDeleted = ccm.IsDeleted,
                    CreatedBy = ccm.CreatedBy,
                    CreatedAt = (DateTime)ccm.CreatedAt,
                    // Add the count of threads
                    ThreadCount = threadGroup.Count()
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

        public async Task<CommunityCategoryMapping> CreateCommunityCategoryMappingAsync(int communityID, CommunityCategoryMappingAPI model)
        {
            var communityCategory = await _context.CommunityCategories
                .FirstOrDefaultAsync(cc => cc.CommunityCategoryID == model.CommunityCategoryID && !cc.IsDeleted);

            var entity = new CommunityCategoryMapping
            {

                CommunityID = communityID,
                CommunityCategoryID = communityCategory.CommunityCategoryID,
                Description = model.Description,
                ModifiedBy = null,
                ModifiedAt = null,
                IsDeleted = false,
                CreatedBy = model.CreatedBy,
                CreatedAt = DateTime.Now
            };

            _context.CommunityCategoryMapping.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<CommunityCategoryMapping> UpdateCommunityCategoryMappingAsync(int communityCategoryMappingID, CommunityCategoryMappingAPI model)
        {
            var entity = await _context.CommunityCategoryMapping
                .FirstOrDefaultAsync(ccm => ccm.CommunityCategoryMappingID == communityCategoryMappingID && !ccm.IsDeleted);
            /*if (entity == null)
                return false;*/
            entity.Description = model.Description;
            entity.ModifiedBy = model.ModifiedBy;
            entity.IsDeleted = false;
            entity.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<CommunityCategoryMapping> DeleteCommunityCategoryMappingAsync(int communityCategoryMappingID)
        {
            var entity = await _context.CommunityCategoryMapping
                .FirstOrDefaultAsync(ccm => ccm.CommunityCategoryMappingID == communityCategoryMappingID && !ccm.IsDeleted);


            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
