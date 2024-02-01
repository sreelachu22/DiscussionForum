﻿using DiscussionForum.Data;
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
    using Azure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.SqlServer.Server;

    using System.Data;

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
        
        
        
        /*GetUserByIDAsync retrieves user details for a specified UserId, 
          including associated department, designation, and role.The method uses 
          a combination of LINQ operations and normal queries to fetch the user 
          and determine their role. If the user doesn't exist, it returns null. 
          The method takes UserId as an input parameter and returns a SingleUserDTO.
          In case of an error, it throws a custom exception.*/
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



        /*PutUserByIDAsync updates a user's role identified by userId, given that the admin user (adminId)
         * has the necessary privileges. The method validates users, roles, and privileges before updating
         * the role mapping. It returns success messages or error messages, such as "Success," "User not found,"
         * "Invalid role ID," or "Insufficient privileges."*/
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

