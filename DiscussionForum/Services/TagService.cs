﻿using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Services
{
    public class TagService : ITagService
    {



        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public TagService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        //Get all tags with pagination
        public async Task<(IEnumerable<TagDto> tagDtos, int totalPages)> GetAllPaginatedTagsAsync(bool isdel, string? sortOrder, string? searchKeyword, int pageNumber, int pageSize)
        {
            try
            {
                // Get tags from database
                var query = _context.Tags.AsQueryable();

                // Filter by isdel
                query = query.Where(tag => tag.IsDeleted == isdel);

                // Apply search filter if searchKeyword is provided
                if (string.IsNullOrEmpty(searchKeyword))
                {
                    searchKeyword = "";
                }
                query = query.Where(tag => tag.TagName.Contains(searchKeyword));

                // Apply sorting based on sortOrder
                if (!string.IsNullOrEmpty(sortOrder) && sortOrder.ToLower() == "desc")
                {
                    query = query.OrderByDescending(tag => tag.TagName);
                }
                else
                {
                    query = query.OrderBy(tag => tag.TagName);
                }

                // Get total count for calculating total pages
                var totalCount = await query.CountAsync();

                // Calculate total pages
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // Apply pagination
                var tags = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                // Map Tag entities to TagDto
                var tagDtos = tags.Select(tag => new TagDto
                {
                    TagId = tag.TagID,
                    TagName = tag.TagName
                    // Map other properties as needed
                });

                return (tagDtos, totalPages);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching tags.", ex);
            }
        }


        /// <summary>
        /// Async get all tags
        /// </summary>
        /// <param name="isdel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<TagDto>> GetAllTagAsync(Boolean isdel)
        {
            try
            {
                return await Task.FromResult(GetAllTags(isdel));
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fectching tags.", ex);
            }
        }


        /// <summary>
        /// Create tags async
        /// </summary>
        /// <param name="tagname"></param>
        /// <param name="createdby"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Tag> CreateTagAsync(string tagname, Guid createdby)
        {
            try
            {
                return await Task.FromResult(CreateTag(tagname, createdby));
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while creating tag.", ex);
            }
        }
        /// <summary>
        /// Tag create function
        /// </summary>
        /// <param name="tagname"></param>
        /// <param name="createdby"></param>
        /// <returns></returns>

        private Tag CreateTag(string tagname, Guid createdby)
        {


            Tag tag = new Tag
            {
                TagName = tagname,
                CreatedBy = createdby,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
            };

            _unitOfWork.Tag.Add(tag);
            _unitOfWork.Complete();
            return tag;
        }


        /// <summary>
        /// Retrives all tags which are not deleted
        /// </summary>
        /// <param name="isdel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private IEnumerable<TagDto> GetAllTags(Boolean isdel)
        {
            try
            {
                var tags = _context.Tags
                            .Where(tag => !tag.IsDeleted)
                            .Select(tag => new TagDto
                            {
                                TagId = tag.TagID,
                                TagName = tag.TagName,
                                TagCount = _context.ThreadTagsMapping
                            .Count(tt => tt.TagID == tag.TagID && !tt.IsDeleted)
                            })
                            .OrderByDescending(tagDto => tagDto.TagCount)
                            .ToList();
                return tags;

            }
            catch (Exception ex)
            {
                throw new Exception("No Tags found.", ex);
            }

        }
        /// <summary>
        /// Retrives tags by async based on seacrh keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<TagDto>> GeAllTagAsync(string keyword)
        {
            try
            {
                return await Task.FromResult(GetAllTags(keyword));
            }
            catch (Exception ex)
            {
                throw new Exception("No Tags found.", ex);
            }
        }

        /// <summary>
        /// Retrives all tags synchrounously based on search keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private IEnumerable<TagDto> GetAllTags(String keyword)
        {
            string lowerkeyword = keyword.ToLower();

            var tags = _context.Tags
                    .Where(tag => tag.TagName.ToLower().Contains(lowerkeyword) && !tag.IsDeleted)
                    .Select(tag => new TagDto
                    {
                        TagId = tag.TagID,
                        TagName = tag.TagName,
                        TagCount = _context.ThreadTagsMapping
                            .Count(tt => tt.TagID == tag.TagID && !tt.IsDeleted)
                    })
                    .OrderByDescending(tagDto => tagDto.TagCount)
                    .ToList();

            return tags;
        }
    }



}
