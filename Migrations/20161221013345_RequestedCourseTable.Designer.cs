using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ContosoUniversity.Data;

namespace ContosoUniversity.Migrations
{
    [DbContext(typeof(SchoolContext))]
    [Migration("20161221013345_RequestedCourseTable")]
    partial class RequestedCourseTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ContosoUniversity.Models.CommitieMembership", b =>
                {
                    b.Property<int>("CommitieMembershipID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CommitteeID");

                    b.Property<int>("PrID");

                    b.HasKey("CommitieMembershipID");

                    b.HasIndex("CommitteeID");

                    b.HasIndex("PrID");

                    b.ToTable("CommitieMembership");
                });

            modelBuilder.Entity("ContosoUniversity.Models.Committee", b =>
                {
                    b.Property<int>("CommitteeID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DepartmentID");

                    b.Property<int?>("FacultyID");

                    b.Property<int>("Level");

                    b.Property<int?>("ProfessorID");

                    b.Property<int>("SemesterID");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("CommitteeID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("FacultyID");

                    b.HasIndex("ProfessorID");

                    b.HasIndex("SemesterID");

                    b.ToTable("Committee");
                });

            modelBuilder.Entity("ContosoUniversity.Models.Course", b =>
                {
                    b.Property<int>("CourseID");

                    b.Property<int>("Credits");

                    b.Property<int>("DepartmentID");

                    b.Property<string>("Title")
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("CourseID");

                    b.HasIndex("DepartmentID");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("ContosoUniversity.Models.CourseAssignment", b =>
                {
                    b.Property<int>("CourseID");

                    b.Property<int>("ProfessorID");

                    b.Property<bool>("Approved");

                    b.Property<DateTime>("AssignmentDate");

                    b.Property<int>("AssignmentID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CourseDescription")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<bool>("CurrentlyTought");

                    b.Property<int?>("SemesterID");

                    b.HasKey("CourseID", "ProfessorID");

                    b.HasIndex("CourseID");

                    b.HasIndex("ProfessorID");

                    b.HasIndex("SemesterID");

                    b.ToTable("CourseAssignment");
                });

            modelBuilder.Entity("ContosoUniversity.Models.CoursePreference", b =>
                {
                    b.Property<int>("CourseID");

                    b.Property<int>("RequestID");

                    b.Property<int>("Choice");

                    b.Property<int?>("RequestProfessorID");

                    b.Property<int?>("RequestSemesterID");

                    b.HasKey("CourseID", "RequestID");

                    b.HasIndex("CourseID");

                    b.HasIndex("RequestSemesterID", "RequestProfessorID");

                    b.ToTable("RequestedCourse");
                });

            modelBuilder.Entity("ContosoUniversity.Models.Department", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AdministratorId");

                    b.Property<decimal>("Budget")
                        .HasColumnType("money");

                    b.Property<int?>("FacultyID");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<int?>("ProfessorID");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<DateTime>("StartDate");

                    b.HasKey("DepartmentID");

                    b.HasIndex("AdministratorId");

                    b.HasIndex("FacultyID");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("ContosoUniversity.Models.Enrollment", b =>
                {
                    b.Property<int>("EnrollmentID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CourseID");

                    b.Property<int?>("Grade");

                    b.Property<string>("Notes");

                    b.Property<int?>("SemesterID");

                    b.Property<int>("SmID");

                    b.HasKey("EnrollmentID");

                    b.HasIndex("CourseID");

                    b.HasIndex("SemesterID");

                    b.HasIndex("SmID");

                    b.ToTable("Enrollment");
                });

            modelBuilder.Entity("ContosoUniversity.Models.OfficeAssignment", b =>
                {
                    b.Property<int>("ProfessorID");

                    b.Property<string>("Location")
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("ProfessorID");

                    b.HasIndex("ProfessorID")
                        .IsUnique();

                    b.ToTable("OfficeAssignment");
                });

            modelBuilder.Entity("ContosoUniversity.Models.SchoolViewModels.DepartmentEmploynment", b =>
                {
                    b.Property<int>("DepartmentID");

                    b.Property<int>("ProfessorID");

                    b.HasKey("DepartmentID", "ProfessorID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("ProfessorID")
                        .IsUnique();

                    b.ToTable("Employments");
                });

            modelBuilder.Entity("ContosoUniversity.Models.TeachingRequest", b =>
                {
                    b.Property<int>("SemesterID");

                    b.Property<int>("ProfessorID");

                    b.Property<string>("Annotation");

                    b.Property<bool>("Approved");

                    b.HasKey("SemesterID", "ProfessorID");

                    b.HasIndex("ProfessorID");

                    b.HasIndex("SemesterID");

                    b.ToTable("TeachingRequest");
                });

            modelBuilder.Entity("ContosoUniversity.Models.UniversityFunctionalityModels.Faculty", b =>
                {
                    b.Property<int>("FacultyID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<int?>("ProfessorID");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("FacultyID");

                    b.HasIndex("ProfessorID");

                    b.ToTable("Faculty");
                });

            modelBuilder.Entity("ContosoUniversity.UniversityFunctionalityModels.Models.Semester", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<bool>("Open");

                    b.Property<int>("Season");

                    b.Property<DateTime>("StartingDate");

                    b.HasKey("ID");

                    b.ToTable("Semester");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int?>("IdentityRole<int>Id");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("IdentityRole<int>Id");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser<int>");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int?>("IdentityUser<int>Id");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("IdentityUser<int>Id");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("ProviderKey");

                    b.Property<string>("LoginProvider");

                    b.Property<int?>("IdentityUser<int>Id");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("ProviderKey", "LoginProvider");

                    b.HasIndex("IdentityUser<int>Id");

                    b.ToTable("UserLogin");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("RoleId");

                    b.Property<int>("UserId");

                    b.Property<int?>("IdentityRole<int>Id");

                    b.Property<int?>("IdentityUser<int>Id");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("IdentityRole<int>Id");

                    b.HasIndex("IdentityUser<int>Id");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId");

                    b.ToTable("UserToken");
                });

            modelBuilder.Entity("ContosoUniversity.Models.Admin", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser<int>");


                    b.ToTable("Admins");

                    b.HasDiscriminator().HasValue("Admin");
                });

            modelBuilder.Entity("ContosoUniversity.Models.Professor", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser<int>");

                    b.Property<int>("DepartmentID");

                    b.Property<string>("FirstMidName")
                        .IsRequired()
                        .HasColumnName("FirstName")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<DateTime>("HireDate");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.ToTable("Professors");

                    b.HasDiscriminator().HasValue("Professor");
                });

            modelBuilder.Entity("ContosoUniversity.Models.Student", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser<int>");

                    b.Property<DateTime>("EnrollmentDate");

                    b.Property<string>("FirstMidName")
                        .IsRequired()
                        .HasColumnName("FirstName")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<bool>("approved");

                    b.ToTable("Student");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("ContosoUniversity.Models.CommitieMembership", b =>
                {
                    b.HasOne("ContosoUniversity.Models.Committee", "Committee")
                        .WithMany("CommitieMembers")
                        .HasForeignKey("CommitteeID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ContosoUniversity.Models.Professor", "Professor")
                        .WithMany("Commities")
                        .HasForeignKey("PrID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ContosoUniversity.Models.Committee", b =>
                {
                    b.HasOne("ContosoUniversity.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentID");

                    b.HasOne("ContosoUniversity.Models.UniversityFunctionalityModels.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyID");

                    b.HasOne("ContosoUniversity.Models.Professor", "Chair")
                        .WithMany()
                        .HasForeignKey("ProfessorID");

                    b.HasOne("ContosoUniversity.UniversityFunctionalityModels.Models.Semester", "Semester")
                        .WithMany("CommitiesRunning")
                        .HasForeignKey("SemesterID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ContosoUniversity.Models.Course", b =>
                {
                    b.HasOne("ContosoUniversity.Models.Department", "Department")
                        .WithMany("Courses")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ContosoUniversity.Models.CourseAssignment", b =>
                {
                    b.HasOne("ContosoUniversity.Models.Course", "Course")
                        .WithMany("Assignments")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ContosoUniversity.Models.Professor", "Professor")
                        .WithMany("Courses")
                        .HasForeignKey("ProfessorID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ContosoUniversity.UniversityFunctionalityModels.Models.Semester")
                        .WithMany("AssignedCourses")
                        .HasForeignKey("SemesterID");
                });

            modelBuilder.Entity("ContosoUniversity.Models.CoursePreference", b =>
                {
                    b.HasOne("ContosoUniversity.Models.Course", "SelectedCourses")
                        .WithMany()
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ContosoUniversity.Models.TeachingRequest", "Request")
                        .WithMany("ListOfCourses")
                        .HasForeignKey("RequestSemesterID", "RequestProfessorID");
                });

            modelBuilder.Entity("ContosoUniversity.Models.Department", b =>
                {
                    b.HasOne("ContosoUniversity.Models.Professor", "Administrator")
                        .WithMany()
                        .HasForeignKey("AdministratorId");

                    b.HasOne("ContosoUniversity.Models.UniversityFunctionalityModels.Faculty")
                        .WithMany("Departments")
                        .HasForeignKey("FacultyID");
                });

            modelBuilder.Entity("ContosoUniversity.Models.Enrollment", b =>
                {
                    b.HasOne("ContosoUniversity.Models.Course", "Course")
                        .WithMany("Enrollments")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ContosoUniversity.UniversityFunctionalityModels.Models.Semester")
                        .WithMany("EnrollmentsInCourses")
                        .HasForeignKey("SemesterID");

                    b.HasOne("ContosoUniversity.Models.Student", "Student")
                        .WithMany("Enrollments")
                        .HasForeignKey("SmID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ContosoUniversity.Models.OfficeAssignment", b =>
                {
                    b.HasOne("ContosoUniversity.Models.Professor", "Professor")
                        .WithOne("OfficeAssignment")
                        .HasForeignKey("ContosoUniversity.Models.OfficeAssignment", "ProfessorID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ContosoUniversity.Models.SchoolViewModels.DepartmentEmploynment", b =>
                {
                    b.HasOne("ContosoUniversity.Models.Department")
                        .WithMany("Staff")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ContosoUniversity.Models.Professor")
                        .WithOne("Employment")
                        .HasForeignKey("ContosoUniversity.Models.SchoolViewModels.DepartmentEmploynment", "ProfessorID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ContosoUniversity.Models.TeachingRequest", b =>
                {
                    b.HasOne("ContosoUniversity.Models.Professor", "ProfessorEntity")
                        .WithMany("TeachingRequests")
                        .HasForeignKey("ProfessorID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ContosoUniversity.UniversityFunctionalityModels.Models.Semester", "SemesterForAssignment")
                        .WithMany()
                        .HasForeignKey("SemesterID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ContosoUniversity.Models.UniversityFunctionalityModels.Faculty", b =>
                {
                    b.HasOne("ContosoUniversity.Models.Professor", "Administrator")
                        .WithMany()
                        .HasForeignKey("ProfessorID");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole<int>")
                        .WithMany("Claims")
                        .HasForeignKey("IdentityRole<int>Id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser<int>")
                        .WithMany("Claims")
                        .HasForeignKey("IdentityUser<int>Id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser<int>")
                        .WithMany("Logins")
                        .HasForeignKey("IdentityUser<int>Id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole<int>")
                        .WithMany("Users")
                        .HasForeignKey("IdentityRole<int>Id");

                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser<int>")
                        .WithMany("Roles")
                        .HasForeignKey("IdentityUser<int>Id");
                });
        }
    }
}
