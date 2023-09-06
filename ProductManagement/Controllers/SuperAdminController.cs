using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data.Repositories;
using ProductManagement.Models.ViewModel;
using ProductManagement.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly ISuperAdminService _superAdminService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SuperAdminController(ISuperAdminService superAdminService, RoleManager<IdentityRole> roleManager)
        {
            _superAdminService = superAdminService;
            _roleManager = roleManager;
        }

        public IActionResult SuperAdminDashboard()
        {
            var admins = _superAdminService.AllAdmins();
            return View(admins);
        }

        public IActionResult UserList()
        {
            var user = _superAdminService.UserLists();
            return View(user);
        }

        public IActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAdmin(AddAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                _superAdminService.AddAdmin(model); 
                return RedirectToAction("SuperAdminDashboard");
            }

            return View(model);
        }
        public IActionResult EditAdmin(Guid id)
        {
            // Retrieve the admin user by ID
            var admin = _superAdminService.GetAdminById(id);

            if (admin != null)
            {
                var editModel = new EditAdminViewModel
                {
                    Id = id,
                    Email = admin.Email,
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Role = admin.Role // Assuming Role is part of admin
                };

                return View(editModel);
            }

            return RedirectToAction("SuperAdminDashboard");
        }

        [HttpPost]
        public IActionResult EditAdmin(EditAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var adminToUpdate = new EditAdminViewModel
                {
                    Id = model.Id,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Role = model.Role
                };

                _superAdminService.EditAdmin(adminToUpdate);

                return RedirectToAction("SuperAdminDashboard");
            }

            return View(model);
        }


        public IActionResult DeleteAdmin(Guid id)
        {
            try
            {
                _superAdminService.DeleteAdmin(id);
                return RedirectToAction("SuperAdminDashboard");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting admin: {ex.Message}");
                return RedirectToAction("SuperAdminDashboard");
            }
        }
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult RoleList()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(model.Name));
            }
            return RedirectToAction("RoleList");
        }


    }
}
