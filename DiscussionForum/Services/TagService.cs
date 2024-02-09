using DiscussionForum.Data;
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
        public async Task<IEnumerable<Tag>> GetAllTagAsync(Boolean isdel)
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

        

        private IEnumerable<Tag> GetAllTags(Boolean isdel)
        {
            if (isdel)
            {
                return _context.Tags.Where(tag => !tag.IsDeleted).ToList();
            }
            else { 
                return _unitOfWork.Tag.GetAll();
            }
            
        }
    }

    


}

