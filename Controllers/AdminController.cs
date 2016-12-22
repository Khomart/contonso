using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ContosoUniversity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContosoUniversity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private SchoolContext _context;

        public AdminController(SchoolContext context,
            UserManager<IdentityUser<int>> userManager,
            SignInManager<IdentityUser<int>> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public UserManager<IdentityUser<int>> _userManager { get; private set; }
        public SignInManager<IdentityUser<int>> _signInManager { get; private set; }

        public async Task<IActionResult> AdminIndex()
        {
            AdminIndex viewModel = new AdminIndex()
            {
                NewStudents = await _context.Students.Where(c => c.approved == false).ToListAsync(),
                Requests = await _context.TeachingRequests.Include(c => c.SemesterForAssignment).Where(c => c.Approved == false && c.SemesterForAssignment.Open == true).ToListAsync(),
                Commities = await _context.Committees.Include(c => c.Chair).Include(c => c.CommitieMembers).ToListAsync(),
            };
            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("StudentApproval")]
        public async Task<IActionResult> StudentApproval()
        {
            int id = int.Parse(Request.Form["ID"]);
            Student user = await _context.Students.Where(i => i.Id == id).SingleOrDefaultAsync();
            user.approved = true;
            await _context.SaveChangesAsync();



            return Json("Nice!");
        }
        [Authorize(Roles = "Admin")]
        [ActionName("CreateCommiteStep1")]
        public async Task<IActionResult> CreateCommiteStep1()
        {
            ViewData["ProfessorID"] = new SelectList(_context.Professors, "Id", "FullName");
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");
            ViewData["FacultyID"] = new SelectList(_context.Facultys.Include(i => i.Departments), "FacultyID", "Name");
            Committee committee = new Committee();
            return View(committee);
        }
    }
}