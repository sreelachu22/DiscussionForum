using DiscussionForum.Data;
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



        //Get All Tags async
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


        //Create tags asyncronously
        public async Task<Tag> CreateTagAsync(string tagname, Guid createdby)
        {
            try
            {
                return await Task.FromResult(CreateTag(tagname,createdby));
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while creating tag.", ex);
            }
        }

       
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

        

        private IEnumerable<TagDto> GetAllTags(Boolean isdel)
        {
            try {
                var tags=_context.Tags
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

        public async Task<IEnumerable<TagDto>> GeAllTagAsync(string keyword)
        {
            try {
                return await Task.FromResult(GetAllTags(keyword));
            }
            catch (Exception ex)
            {
                throw new Exception("No Tags found.", ex);
            }
        }

        private IEnumerable<TagDto> GetAllTags(String keyword)
        { 
            string lowerkeyword=keyword.ToLower();

            var tags = _context.Tags
                    .Where(tag => tag.TagName.ToLower().Contains(lowerkeyword) && !tag.IsDeleted)
                    .Select(tag => new TagDto
                    {
                        TagId=tag.TagID,
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

