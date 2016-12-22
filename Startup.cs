using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using ContosoUniversity.UniversityFunctionalityModels.Models;
//using Microsoft.AspNetCore.Owin;

namespace ContosoUniversity
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<SchoolContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser<int>, IdentityRole<int>>(options => {

                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromHours(1);

            })
                .AddEntityFrameworkStores<SchoolContext, int>()
                .AddDefaultTokenProviders();
            services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<SchoolContext, int>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // Cookie settings
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";
                options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOff";

                // User settings
                options.User.RequireUniqueEmail = true;
            });
            //services.AddIdentity<Professor, IdentityRole>()
            //.AddEntityFrameworkStores<SchoolContext>()
            //.AddDefaultTokenProviders();

            services.AddMvc();
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();
            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, SchoolContext context)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "Student",
                   template: "{action}/{id?}",
                   defaults: new { controller = "Student" });
                routes.MapRoute(
                   name: "Professor",
                   template: "{action}/{id?}",
                   defaults: new { controller = "Professor" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
                routes.MapRoute(
                    name: "index",
                    template: "{action}",
                    defaults: new { controller = "Home", action = "Index" });
                routes.MapRoute(
                    name: "CourseRequest",
                    template: "{controller}/{action}/{SemesterID}/{ProfessorID}",
                    defaults: new { controller = "Professor", action = "MyRequest" });

            });
            //context.Database.ExecuteSqlCommand("DELETE FROM [AspNetRoles]");
            //context.Database.ExecuteSqlCommand("DELETE FROM [AspNetUsers]");
            //context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('aspnet-ContosoUniversity-1e12bc29-9eea-4b8e-9282-915478be1be9.dbo.AspNetRoles',RESEED, 0)");
            //context.Database.ExecuteSqlCommand("TRUNCATE TABLE [AspNetUsers]");

            createRolesandUsers.CreateRoles(context, app.ApplicationServices).Wait();
            year.CreateTerm(context).Wait();
            DbInitializer.Initialize(context);
        }
        //

        private static class year{
            public static async Task CreateTerm(SchoolContext context)
            {
                DateTime currentYear = DateTime.Now;
                var contextSems = context.Semesters.Where(m => m.StartingDate.Year == currentYear.Year);
                if (contextSems.Count() == 0)
                {
                    for (int i = 1; i < 4; i++)
                    {
                        Semester term = new Semester()
                        {

                            StartingDate = currentYear,
                            EndDate = currentYear.AddYears(1),
                            Open = true,
                            Season = (Term)i,
                        };
                        if (i == 3) term.StartingDate = currentYear.AddYears(1);
                        context.Semesters.Add(term);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
        //public class RoleConstraint : IRouteConstraint
        //{

        //    public bool Match(
        //        HttpContext httpContext,
        //        Route route,
        //        string parameterName,
        //        RouteValueDictionary values,
        //        RouteDirection routeDirection, 
        //        IServiceProvider serviceProvider)
        //    {
        //        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser<int>>>();
        //        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        //        RoleProvider rp = new tst.Providers.CustomRoleProvider();
        //        string[] roles = rp.GetRolesForUser(httpContext.User.Identity.Name);
        //        if (roles != null && roles.Length > 0)
        //        {
        //            string roleName = roles[0];
        //            string areaName = route.Defaults["area"].ToString();
        //            return areaName == roleName;
        //        }
        //        return false;
        //    }
        //}


        private static class createRolesandUsers
        {
            public static async Task CreateRoles(SchoolContext context, IServiceProvider serviceProvider)
            {

                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser<int>>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                // First, Creating User role as each role in User Manager  
                List<IdentityRole<int>> roles = new List<IdentityRole<int>>();

                roles.Add(new IdentityRole<int> { Name = "Student", NormalizedName = "STUDENT" });
                roles.Add(new IdentityRole<int> { Name = "Registered", NormalizedName = "REGISTERED" });
                roles.Add(new IdentityRole<int> { Name = "Admin", NormalizedName = "ADMINISTRATOR" });
                roles.Add(new IdentityRole<int> { Name = "Professor", NormalizedName = "PROFESSOR" });

                //Then, the machine added Default User as the Admin user role
                foreach (var role in roles)
                {
                    var roleExit = await roleManager.RoleExistsAsync(role.Name);
                    if (!roleExit)
                    {
                        var result = await roleManager.CreateAsync(role);
                    }
                }

                await CreateUser(context, userManager);
            }


            private static async Task CreateUser(SchoolContext context, UserManager<IdentityUser<int>> userManager)
            {
                var adminUser = await userManager.FindByEmailAsync("admin@university.com");
                if (adminUser != null)
                {
                    if (!(await userManager.IsInRoleAsync(adminUser, "Admin")))
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    var newAdmin = new Admin
                    {
                        UserName = "YourMainAdmin",
                        Email = "admin@university.com",

                    };
                    var result = await userManager.CreateAsync(newAdmin, "Ceh;br196240");
                    if (!result.Succeeded)
                    {
                        var exceptionText = result.Errors.Aggregate("User Creation Failed - Identity Exception. Errors were: \n\r\n\r", (current, error) => current + (" - " + error + "\n\r"));
                        throw new Exception(exceptionText);
                    }
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }

        }




    }
}
