using CourseManagementSystem.Data;
using CourseManagementSystem.Helpers;
using CourseManagementSystem.Models;
using CourseManagementSystem.Repositories;
using CourseManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace CourseManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add environment variables to configuration
            builder.Configuration.AddEnvironmentVariables();


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            //Configuring the EFCore to use ApplicationDbContext as the context class model
            builder.Services.AddDbContext<ApplicationDbContext> (
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                }
            );

            //Configuring UserManger and RoleManager and thier stores for Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true;
                }


            ).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", options =>
                {
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });

            //Configure class to map to mail settings in appsettings.json
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

            //Add Services for Dependency Injecttion
            builder.Services.AddScoped<IMajorRepository, MajorRepository>();
            builder.Services.AddScoped<IMajorService, MajorService>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            builder.Services.AddScoped<IAssignmentService, AssignmentService>();
            builder.Services.AddScoped<IGradingRepository, GradingRepository>();
            builder.Services.AddScoped<IGradingService, GradingService>();
            builder.Services.AddScoped<ILectureFilesRepository,LectureFilesRepository>();
            builder.Services.AddScoped<ILectureFilesService, LectureFilesService>();
            builder.Services.AddScoped<IEnrolmentRepository, EnrollmentRepository>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<IMailService, MailService>();

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePagesWithReExecute("/Home/NotFound", "?statusCode={0}");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
                
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
