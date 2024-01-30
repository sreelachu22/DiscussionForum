using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

using DiscussionForum.Repositories;

using DiscussionForum.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace DiscussionForum.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _userContext;


        public UserService(AppDbContext userContext)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));

        }

        public async Task<PagedUserResult> GetUsers(string? term, string? sort, int page, int limit)
        {
            try
            {
                term = string.IsNullOrWhiteSpace(term) ? null : term.Trim().ToLower();

                var users = _userContext.Users
                    .Where(u =>
                        string.IsNullOrWhiteSpace(term) ||
                        u.Name.ToLower().Contains(term) ||
                        u.Email.ToLower().Contains(term)
                    );

                if (!string.IsNullOrWhiteSpace(sort))
                {
                    var sortFields = sort.Split(',');
                    StringBuilder orderQueryBuilder = new StringBuilder();
                    PropertyInfo[] propertyInfo = typeof(User).GetProperties();

                    foreach (var field in sortFields)
                    {
                        string sortOrder = "ascending";
                        var sortField = field.Trim();
                        if (sortField.StartsWith("-"))
                        {
                            sortField = sortField.TrimStart('-');
                            sortOrder = "descending";
                        }
                        var property = propertyInfo.FirstOrDefault(a => a.Name.Equals(sortField, StringComparison.OrdinalIgnoreCase));
                        if (property == null)
                            continue;
                        orderQueryBuilder.Append($"{property.Name.ToString()} {sortOrder}, ");
                    }

                    if (orderQueryBuilder.Length > 0)
                    {
                        orderQueryBuilder.Remove(orderQueryBuilder.Length - 2, 2);
                        users = users.OrderBy(orderQueryBuilder.ToString());
                    }
                    else
                    {
                        users = users.OrderBy(u => u.UserID);
                    }
                }

                var totalCount = await users.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalCount / limit);

                var pagedUsers = await users.Skip((page - 1) * limit).Take(limit).ToListAsync();

                var pagedUserData = new PagedUserResult
                {
                    Users = pagedUsers,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                };
                return pagedUserData;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw;
            }
        }

        public async Task<SingleUserDTO> GetUserByIDAsync(Guid UserId)
        {
            try
            {
                

                var user = await _userContext.Users
                    .Include(u => u.Department)
                    .Include(u => u.Designation)
                    .FirstOrDefaultAsync(u => u.UserID == UserId);

                if (user == null)
                {
                    return null;
                }

               /* var roleName = (from rm in _userContext.UserRoleMapping
                                join r in _userContext.Roles on rm.RoleID equals r.RoleID
                                where rm.UserID == UserId
                                select r.RoleName)
                                .FirstOrDefault();*/

                var roleName = _userContext.UserRoleMapping
                                .Where(rm => rm.UserID == UserId)
                                .Join(_userContext.Roles, rm => rm.RoleID, r => r.RoleID, (rm, r) => r.RoleName)
                                .FirstOrDefault();

                var userDto = new SingleUserDTO
                {
                    UserID = user.UserID,
                    Name = user.Name,
                    Email = user.Email,
                    Score = user.Score,
                    DepartmentName = user.Department.DepartmentName,
                    DesignationName = user.Designation.DesignationName,
                    RoleName = roleName
                };

                return userDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching User.", ex);
            }
        }

    }


}

