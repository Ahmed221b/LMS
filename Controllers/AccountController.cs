using CourseManagementSystem.Data;
using CourseManagementSystem.Models;
using CourseManagementSystem.Services;
using CourseManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IMajorService _majorService;
        private readonly IMailService mailService;
        public AccountController(IMailService mailService,IMajorService majorService,ApplicationDbContext dbContext,RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = dbContext;
            _majorService = majorService;
            this.mailService = mailService;
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [Authorize(Roles =("Admin"))]
        [HttpGet]
        public async Task<IActionResult> CreateAccount()
        {
            var rolesNames = await _roleManager.Roles.Select(n => n.Name).ToListAsync();
            ViewBag.Roles = new SelectList(rolesNames, rolesNames);
            ViewBag.Majors = new SelectList(_majorService.GetAllMajors(), "Id", "Name");
            return View();
        }

        [Authorize(Roles = ("Admin"))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = null;

                switch (viewModel.RoleName)
                {
                    case "Admin":
                        var admin = new Admin
                        {
                            FirstName = viewModel.FirstName,
                            LastName = viewModel.LastName,
                            Email = viewModel.Email,
                            Address = viewModel.Address,
                            PhoneNumber = viewModel.PhoneNumber,
                            DataOfBirth = viewModel.DateOfBirth,
                            UserName = (viewModel.FirstName + viewModel.LastName).ToLower()
                        };
                        result = await _userManager.CreateAsync(admin, viewModel.Password);
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(admin, viewModel.RoleName);
                        }
                        break;

                    case "Student":
                        var student = new Student
                        {
                            FirstName = viewModel.FirstName,
                            LastName = viewModel.LastName,
                            Email = viewModel.Email,
                            Address = viewModel.Address,
                            PhoneNumber = viewModel.PhoneNumber,
                            DataOfBirth = viewModel.DateOfBirth,
                            UserName = (viewModel.FirstName + viewModel.LastName).ToLower(),
                            CGPA = 0,
                            MajorId = viewModel.MajorId ??  0,
                            Courses = new List<Course>(),
                            AssignmentSubmissions = new List<AssignmentSubmission>(),

                        };
                        result = await _userManager.CreateAsync(student, viewModel.Password);
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(student, viewModel.RoleName);
                        }
                        break;

                    case "Instructor":
                        var instructor = new Instructor
                        {
                            FirstName = viewModel.FirstName,
                            LastName = viewModel.LastName,
                            Email = viewModel.Email,
                            Address = viewModel.Address,
                            PhoneNumber = viewModel.PhoneNumber,
                            DataOfBirth = viewModel.DateOfBirth,
                            UserName = (viewModel.FirstName + viewModel.LastName).ToLower(),
                            Courses = new List<Course>()
                        };
                        result = await _userManager.CreateAsync(instructor, viewModel.Password);
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(instructor, viewModel.RoleName);
                        }
                        break;

                    default:
                        break;
                }

                if (result != null && result.Succeeded)
                {
                    ViewBag.Created = true;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (result != null)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    ViewBag.Created = false;
                }
            }

            var rolesNames = await _roleManager.Roles.Select(n => n.Name).ToListAsync();
            ViewBag.Roles = new SelectList(rolesNames, rolesNames);
            ViewBag.Majors = new SelectList(_majorService.GetAllMajors(),"Id","Name");
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(viewModel.UserName);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, isPersistent: viewModel.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        if (User.IsInRole("Admin"))
                            return RedirectToAction("Index", "Admin");

                        else
                            return RedirectToAction("Index", "Home");
                           

                    }
                    else if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "Account is locked out");
                        return View(viewModel);
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid log in attempt");

            }
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(viewModel.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account", 
                        new {email = viewModel.Email,token = token},Request.Scheme);

                    // Send the reset password email
                    var subject = "Reset Your Password";
                    var body = $"Please reset your password by clicking the following link: <a href='{passwordResetLink}'>Reset Password</a>";

                    await mailService.SendMail(viewModel.Email, subject, body);

                    return View("ForgotPasswordConfirmation");
                }

                return View("ForgotPasswordConfirmation");
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ResetPassword(string email,string token)
        {
            if (email is null || token is null)
            {
                ModelState.AddModelError("","Invalid Reset token");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(viewModel.Email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user,viewModel.Token,viewModel.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    
                    foreach (var error in  result.Errors)
                    {
                            ModelState.AddModelError("", error.Description);
                    }
                    return View(viewModel);
                    
                }
                return View("ResetPasswordConfirmation");

            }
            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var viewModel = new ProfileViewModel
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DataOfBirth,
                CGPA = null
            };

            if (await _userManager.IsInRoleAsync(user, "Student"))
            {
                var student = await _userManager.FindByIdAsync(user.Id) as Student;
                if (student != null)
                {
                    viewModel.CGPA = student.CGPA;
                }
            }


            return View(viewModel);
        }


        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user =await _userManager.GetUserAsync(User);
                if (user is null)
                    return RedirectToAction("Login");

                var result =  await _userManager.ChangePasswordAsync(user, viewModel.CurrentPassword, viewModel.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(viewModel);
                }
                await _signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }
            return View(viewModel);
            
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return RedirectToAction("Login");

            var model =new  EditProfileViewModel{
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Address = user.Address,
                PhoneNumer = user.PhoneNumber,
                DateOfBirth = user.DataOfBirth,
                Email = user.Email
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user is null)
                    return RedirectToAction("Login");

                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                user.UserName = viewModel.UserName;
                user.Address = viewModel.Address;
                user.PhoneNumber = viewModel.PhoneNumer;
                user.Email = viewModel.Email;
                user.DataOfBirth = viewModel.DateOfBirth;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(viewModel);
                }

                
            }
            return View(viewModel);
        }


        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}

