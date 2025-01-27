﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TechShop.Application.Services.UserServices.UserProfileService;
using TechShop.Domain.Entities;
using TechShop.Domain.Entities.UserEntities;
using TechShop.Domain.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechShop.Infrastructure.SeedData
{
    public static class CreateDefaultUsersExtension 
    {
        public static async Task CreateDefaultUsers(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();

            var userProfileService = scope.ServiceProvider.GetRequiredService<IUserProfileService>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();         

            var adminUser = await userManager.FindByEmailAsync(DefaultUsers.AdminEmail);
            var defaultUser = await userManager.FindByEmailAsync(DefaultUsers.UserEmail);

            if (adminUser == null)
            {             
                var admin = new ApplicationUser
                {
                    Email = DefaultUsers.AdminEmail,
                    UserName = DefaultUsers.AdminName,
                };               

                var adminResponse = await userManager.CreateAsync(admin, DefaultUsers.AdminPassword);

                if (adminResponse.Succeeded)
                {
                    var date = DateTime.Now;
                    await userManager.AddToRoleAsync(admin, DefaultRoles.Admin);
                    var adminProfile = new UserProfile
                    {
                        UserId = admin.Id,
                        Firstname = "Admin",
                        Lastname = "Admin",
                        Age = 20,
                        LastLogin = date,
                        CreatedDate = date,
                        UpdatedDate = date,
                    };
                    await userProfileService.AddAsync(adminProfile);
                }

                
            }

            if (defaultUser == null)
            {
                var user = new ApplicationUser
                {
                    Email = DefaultUsers.UserEmail,
                    UserName = DefaultUsers.UserName,
                };

                var userResponse = await userManager.CreateAsync(user, DefaultUsers.UserPassword);
                if (userResponse.Succeeded)
                {
                    var date = DateTime.Now;
                    await userManager.AddToRoleAsync(user, DefaultRoles.User);
                    var userProfile = new UserProfile
                    {
                        UserId = user.Id,
                        Firstname = "User",
                        Lastname = "User",
                        Age = 20,
                        LastLogin = date,
                        CreatedDate = date,
                        UpdatedDate = date,
                    };
                    await userProfileService.AddAsync(userProfile);
                }
            }
        }
    }
}
