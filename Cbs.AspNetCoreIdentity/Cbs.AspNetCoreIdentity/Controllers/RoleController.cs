using Cbs.AspNetCoreIdentity.Entities;
using Cbs.AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cbs.AspNetCoreIdentity.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {

            var list = _roleManager.Roles.ToList();
            return View(list);
        }
        public IActionResult Create()
        {
            return View(new RoleModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                AppRole appRole = new()
                {
                    Name = model.Name,
                    CreatedTime = DateTime.UtcNow,
                    NormalizedName = model.Name.ToUpper(),

            };
                //model.CreatedTime = DateTime.UtcNow;
                //model.NormalizedName = model.Name.ToUpper();

            var result =    await _roleManager.CreateAsync(appRole);
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
    }
}
