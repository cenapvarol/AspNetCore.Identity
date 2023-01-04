﻿using Cbs.AspNetCoreIdentity.Context;
using Cbs.AspNetCoreIdentity.Entities;
using Cbs.AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbs.AspNetCoreIdentity.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly CbsContext _cbsContext;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, CbsContext cbsContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _cbsContext = cbsContext;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var query = _userManager.Users;
            var users = _cbsContext.Users.Join(_cbsContext.UserRoles, user => user.Id, userRole => userRole.UserId, (user, userRole) => new
            {
                user,
                userRole
            }).Where(x => x.userRole.RoleId != 1).Select(x => new AppUser
            {
                Id = x.user.Id,
                AccessFailedCount = x.user.AccessFailedCount,
                ConcurrencyStamp = x.user.ConcurrencyStamp,
                Email = x.user.Email,
                EmailConfirmed = x.user.EmailConfirmed,
                Gender = x.user.Gender,
                ImagePath = x.user.ImagePath,
                LockoutEnabled = x.user.LockoutEnabled,
                LockoutEnd = x.user.LockoutEnd,
                NormalizedEmail = x.user.NormalizedEmail,
                NormalizedUserName = x.user.NormalizedUserName,
                PasswordHash = x.user.PasswordHash,
                PhoneNumber = x.user.PhoneNumber,
                UserName = x.user.UserName,

            }).ToList();
            //var users = await _userManager.GetUsersInRoleAsync("Member");
            return View(users);
        }
        public IActionResult Create()
        {
            return View(new UserAdminCreateModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserAdminCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var user =  new AppUser
                {
                    Email = model.Email,
                    Gender = model.Gender,
                    UserName = model.UserName
                };
             var result =   await _userManager.CreateAsync(user,model.UserName+"123");
                await _userManager.AddToRoleAsync(user, "Member");
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);

                }


            }
            return View(model);
        }
        public async Task<IActionResult> AssignRole(int id)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            var userRole =await  _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.ToList();

            RoleAssignSendModel model = new RoleAssignSendModel();

            List<RoleAssignListModel> list = new List<RoleAssignListModel>();

            foreach (var role in roles)
            {
                list.Add(new()
                {
                    Name = role.Name,
                    RoleId = role.Id,
                    Exist = userRole.Contains(role.Name)
                });
            }
            model.Roles = list;
            model.UserId = id;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(RoleAssignSendModel model)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == model.UserId);
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in model.Roles)
            {
                if (role.Exist)
                {
                    if (!userRoles.Contains(role.Name))
                    {
                       await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else
                    {
                        if (userRoles.Contains(role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(user, role.Name);
                        }
                    }
                }

            }
            return RedirectToAction("Index");
        }
    }
}
