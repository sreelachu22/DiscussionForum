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
    using Microsoft.AspNetCore.Mvc;
    using System.Data.Common;

    public class UserService : IUserService
    {
        private readonly AppDbContext _userContext;

        public UserService(AppDbContext userContext)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));

        }

        //this function is used to return all the users with the option to paginate and sort
        //sort variable takes the member variable according to which sort is done
        //term variable is used as a search term to filter users in a case-insensitive manner.
        //page is for defining the page to return after pagination
        //limit is to specify the number of users needed to show in a page
        public async Task<PagedUserResult> GetUsers(string? term, string? sort, int page, int limit)
        {
            try
            {
                // Normalize and clean up the search term
                term = string.IsNullOrWhiteSpace(term) ? null : term.Trim().ToLower();

                // Build the initial query based on search term
                var usersQuery = _userContext.Users
                    .Where(u =>
                        string.IsNullOrWhiteSpace(term) ||
                        u.Name.ToLower().Contains(term)
                        /*u.Email.ToLower().Contains(term)*/
                    );

                // Apply sorting if specified
                if (!string.IsNullOrWhiteSpace(sort))
                {
                    var sortFields = sort.Split(',');
                    var orderQueryBuilder = new StringBuilder();
                    var propertyInfo = typeof(User).GetProperties();

                    // Construct the order-by clause based on sort fields
                    foreach (var field in sortFields)
                    {
                        var sortOrder = field.StartsWith("-") ? "descending" : "ascending";
                        var sortField = field.TrimStart('-');
                        var property = propertyInfo.FirstOrDefault(a => a.Name.Equals(sortField, StringComparison.OrdinalIgnoreCase));

                        if (property != null)
                            orderQueryBuilder.Append($"{property.Name} {sortOrder}, ");
                    }

                    // Apply sorting to the query
                    if (orderQueryBuilder.Length > 0)
                    {
                        orderQueryBuilder.Length -= 2; // Remove the trailing comma and space
                        usersQuery = usersQuery.OrderBy(orderQueryBuilder.ToString());
                    }
                    else
                    {
                        usersQuery = usersQuery.OrderBy(u => u.UserID);
                    }
                }
                
                var totalCount = await usersQuery.CountAsync();
                
                var totalPages = (int)Math.Ceiling((double)totalCount / limit);
                
                var pagedUsers = await usersQuery.Skip((page - 1) * limit).Take(limit).ToListAsync();
                
                return new PagedUserResult
                {
                    Users = pagedUsers,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                };
            }
            catch (DbException ex)
            {                
                Console.WriteLine($"Database error occurred: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {                
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }





        /* get single user details*/
        public async Task<SingleUserDTO> GetUserByIDAsync(Guid UserId)
        {
            try
            {

                /* user exists?*/
                var user = await _userContext.Users
                    .Include(u => u.Department)
                    .Include(u => u.Designation)
                    .FirstOrDefaultAsync(u => u.UserID == UserId);

                if (user == null)
                {
                    return null;
                }

                /* linq operation and normal opertation for geting rolename*/

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


        /* edit single user role*/

        public async Task<String> PutUserByIDAsync(Guid userId, int roleID, Guid adminId)
        {
            try
            {
                // Check if the user exists
                var userExists = await _userContext.Users.AnyAsync(u => u.UserID == userId);
                if (!userExists)
                {
                    return ("User not found");
                }

                // Check if the admin user exists
                var adminExists = await _userContext.Users.AnyAsync(u => u.UserID == adminId);

                if (!adminExists)
                {
                    return ("Admin User not found or doesn't have the privilege");
                }

                // Check if the admin user has the privilege (role ID is 1 or 2)
                var isAdminUser = await _userContext.UserRoleMapping
                    .Where(ur => ur.UserID == adminId)
                    .Select(ur => ur.RoleID)
                    .AnyAsync(roleId => roleId == 1 || roleId == 2);

                if (!isAdminUser)
                {
                    return ("Admin user doesn't have the privilege");
                }


                //Admin cant change his role
                if (adminId == userId) {
                    return ("Not allowed");
                }

                // Check if the provided role ID exists in the Roles table and is either 1 or 2
                var roleExists = await _userContext.Roles.AnyAsync(r => r.RoleID == roleID && (r.RoleID == 2 || r.RoleID == 3));

                if (!roleExists)
                {
                    return "Invalid role ID or not allowed to change to the specified role";
                }

                // Get the user role mapping
                var userRoleMapping = await _userContext.UserRoleMapping
                                    .Where(ur => ur.UserID == userId)
                                    .FirstOrDefaultAsync();

                // Update the user role mapping
                userRoleMapping.RoleID = roleID;
                userRoleMapping.ModifiedBy = adminId;
                userRoleMapping.ModifiedAt = DateTime.UtcNow;

                // Save changes to the database
                await _userContext.SaveChangesAsync();

                // Return success response
                return ("Success");
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return (ex.Message);
            }
        }


        //Leaderboard

        public async Task<List<SingleUserDTO>> GetTopUsersByScoreAsync(int limit)
        {
            var topUsers = await _userContext.Users
                .Where(u => !u.IsDeleted)
                .OrderByDescending(u => u.Score)
                .Take(limit)
                .Join(_userContext.Departments, u => u.DepartmentID, d => d.DepartmentID, (u, d) => new { User = u, Department = d })
                .Join(_userContext.Designations, u => u.User.DesignationID, des => des.DesignationID, (ud, des) => new { User = ud, Designation = des })
                .Select(ud => new SingleUserDTO
                {
                    UserID = ud.User.User.UserID,
                    Name = ud.User.User.Name,
                    Email = ud.User.User.Email,
                    Score = ud.User.User.Score,
                    DepartmentName = ud.User.Department.DepartmentName,
                    DesignationName = ud.Designation.DesignationName // Assuming you have a navigation property to UserRole
                })
                .ToListAsync();

            return topUsers;
        }

    }


}

