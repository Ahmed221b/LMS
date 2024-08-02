using CourseManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Calling the base OnModelCreating 
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Instructor>().ToTable("Instructors");


            //Defining the self-refrencing many to many relationship between Course and itself
            modelBuilder.Entity<PrerequisiteCourse>()
                .HasKey(p => new { p.PrerequisiteId, p.CourseId });

            modelBuilder.Entity<PrerequisiteCourse>()
                .HasOne(c => c.Course)
                .WithMany(p => p.PrerequisiteCourses)
                .HasForeignKey(f => f.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PrerequisiteCourse>()
                .HasOne(c => c.Prerequisite)
                .WithMany(p => p.DependentCourses)
                .HasForeignKey(f => f.PrerequisiteId)
                .OnDelete(DeleteBehavior.Restrict);

            //Defining the relationship between Course and Student Many To Many

            modelBuilder.Entity<Course>()
                .HasMany(p => p.Students)
                .WithMany(p => p.Courses);


            modelBuilder.Entity<Grade>()
                .HasOne(p => p.Student)
                .WithMany(p => p.Grades)
                .HasForeignKey(p => p.StudentId);
            
            modelBuilder.Entity<Grade>()
                .HasOne(p => p.Course)
                .WithMany(p => p.Grades)
                .HasForeignKey(p => p.CourseId);

           

       



            //Defining the relationship between Instructor and Course one to Many
            modelBuilder.Entity<Instructor>()
                .HasMany(p => p.Courses)
                .WithOne(p => p.Instructor)
                .HasForeignKey(p => p.InstructorId)
                .OnDelete(DeleteBehavior.SetNull);

            //Defining the relationship between Course and Assignment one to many
            modelBuilder.Entity<Assignment>()
                .HasOne(p => p.Course)
                .WithMany(p => p.Assignments)
                .HasForeignKey(p => p.CourseId);
            
            //Defining the relationship between Major and Course one to many
            modelBuilder.Entity<Major>()
                .HasMany(p => p.Courses)
                .WithOne(p => p.Major)
                .HasForeignKey(p => p.MajorId);

            //Defining the relationship between Assinment,Student,Course and AssignmentSubmissions

            modelBuilder.Entity<AssignmentSubmission>()
                .HasKey(p => new { p.StudentId, p.CourseId, p.AssignmentId });

            modelBuilder.Entity<Assignment>()
                .HasMany(p => p.AssignmentSubmissions)
                .WithOne(a => a.Assignment)
                .HasForeignKey(p => p.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AssignmentSubmission>()
                .HasOne(p => p.Course)
                .WithMany(p => p.AssignmentSubmissions)
                .HasForeignKey(p => p.CourseId);

            modelBuilder.Entity<AssignmentSubmission>()
                .HasOne(p => p.Student)
                .WithMany(s => s.AssignmentSubmissions)
                .HasForeignKey(p => p.StudentId);

            //Defining relationship between Course and LectureFile one to many
            modelBuilder.Entity<Course>()
                .HasMany(p => p.LectureFiles)
                .WithOne(p => p.Course)
                .HasForeignKey(k => k.CourseId);

            //Defining the relationship between Student and Major one to many
            modelBuilder.Entity<Student>()
                .HasOne(p => p.Major)
                .WithMany(p => p.Students)
                .HasForeignKey(c => c.MajorId);

            //Setting Major Id as identity auto genertated value because it's byte
       
            modelBuilder.Entity<Major>()
                .Property(k => k.Id)
                .ValueGeneratedOnAdd();


            
                
        }


        //Tables of the System
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<PrerequisiteCourse> PrerequisiteCourse { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<LectureFile> LectureFile { get; set; }

    }
}
