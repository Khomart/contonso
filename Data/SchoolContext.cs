using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ContosoUniversity.UniversityFunctionalityModels.Models;
using ContosoUniversity.Models.SchoolViewModels;
using ContosoUniversity.Models.UniversityFunctionalityModels;

namespace ContosoUniversity.Data
{
    public class SchoolContext : IdentityDbContext< IdentityUser<int>, IdentityRole<int>, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Faculty> Facultys { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<Committee> Committees { get; set; }
        public DbSet<CommitieMembership> CommitieMembership { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<TeachingRequest> TeachingRequests { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<DepartmentEmploynment> Employments { get; set; }
        public DbSet<CoursePreference> RequestedCourses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser<int>>(i => {
                i.ToTable("Users");
                i.HasKey(x => x.Id);
            });
            modelBuilder.Entity<IdentityRole<int>>(i => {
                i.ToTable("Role");
                i.HasKey(x => x.Id);
            });
            modelBuilder.Entity<IdentityUserRole<int>>(i => {
                i.ToTable("UserRole");
                i.HasKey(x => new { x.RoleId, x.UserId });
            });
            modelBuilder.Entity<IdentityUserLogin<int>>(i => {
                i.ToTable("UserLogin");
                i.HasKey(x => new { x.ProviderKey, x.LoginProvider });
            });
            modelBuilder.Entity<IdentityRoleClaim<int>>(i => {
                i.ToTable("RoleClaims");
                i.HasKey(x => x.Id);
            });
            modelBuilder.Entity<IdentityUserClaim<int>>(i => {
                i.ToTable("UserClaims");
                i.HasKey(x => x.Id);
                
            });
            modelBuilder.Entity<IdentityUserToken<int>>(i => {
                i.ToTable("UserToken");
                i.HasKey(x => new {  x.UserId });
            });
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Faculty>().ToTable("Faculty");
            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment");
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment");
            modelBuilder.Entity<DepartmentEmploynment>().ToTable("Employments").HasKey(c=> new { c.DepartmentID, c.ProfessorID });
            modelBuilder.Entity<Committee>().ToTable("Committee");
            modelBuilder.Entity<CommitieMembership>().ToTable("CommitieMembership");
            modelBuilder.Entity<Professor>().ToTable("Professors");
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<TeachingRequest>().ToTable("TeachingRequest").HasKey(c => new { c.SemesterID, c.ProfessorID });
            modelBuilder.Entity<Semester>().ToTable("Semester");
            modelBuilder.Entity<CoursePreference>().ToTable("RequestedCourse").HasKey(c => new { c.CourseID, c.RequestID });

            modelBuilder.Entity<CourseAssignment>()
                .HasKey(c => new { c.CourseID, c.ProfessorID });


        }
    }
}