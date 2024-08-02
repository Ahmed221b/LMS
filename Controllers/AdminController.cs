using CourseManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Versioning;

namespace CourseManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Accounts(string roleName)
        {
            roleName = roleName ?? "All";
            List<ApplicationUser> users = new List<ApplicationUser>();
            switch (roleName) 
            {
                case "All":
                    users = userManager.Users.ToList();
                    break;
                case "Admin":
                    users = await GetUsersInRoleAsync(roleName);
                    break;
                case "Student":
                    users = await GetUsersInRoleAsync(roleName);
                    break;
                case "Instructor":
                    users = await GetUsersInRoleAsync(roleName);
                    break;
            }
            var roles = roleManager.Roles.Select(s => s.Name).ToList();
            ViewBag.Roles = new SelectList(roles, roles);
            ViewBag.SelectedRole = roleName;
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {

            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Accounts");
                }
            }
            return RedirectToAction("Accounts");
        }
   

        private async Task<List<ApplicationUser>> GetUsersInRoleAsync(string roleName)
            => (List<ApplicationUser>)await userManager.GetUsersInRoleAsync(roleName);


       
    }
}
