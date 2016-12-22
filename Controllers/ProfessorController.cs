using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ContosoUniversity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using ContosoUniversity.UniversityFunctionalityModels.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContosoUniversity.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController : Controller
    {
        private SchoolContext _context;

        public ProfessorController(SchoolContext context,
            UserManager<IdentityUser<int>> userManager,
            SignInManager<IdentityUser<int>> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public UserManager<IdentityUser<int>> _userManager { get; private set; }
        public SignInManager<IdentityUser<int>> _signInManager { get; private set; }

        public async Task<IActionResult> ProfessorIndex()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var viewModel = new ProfessorViews();
            viewModel.Professors = await _context.Professors
                //.Include(s => s.Courses)
                //    .ThenInclude(e => e.Course)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == user.Id);
            viewModel.CoursesAssignments =  _context.CourseAssignments.Include(c => c.Course).AsNoTracking().Where(s => s.ProfessorID == user.Id).ToList();
            viewModel.Requests = _context.TeachingRequests.Include(c => c.SemesterForAssignment).Where(s => s.ProfessorID == user.Id).AsNoTracking().ToList();
                ViewData["ProfessorID"] = user.Id;
            viewModel.Membership = _context.CommitieMembership.Where(i => i.PrID == user.Id).ToList();
            var semesters = _context.Semesters.Where(m => m.Open == true);
            ViewBag.Semesters = new SelectList(semesters, "ID", "Title");



            ViewData["Message"] = "Default for professor.";
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //var professor = await _context.Professors
            //    .Include(s => s.Courses)
            //        .ThenInclude(e => e.Course)
            //    .AsNoTracking()
            //    .SingleOrDefaultAsync(m => m.Id == user.Id);

            //if (professor == null)
            //{
            //    return NotFound();
            //}

            return View(viewModel);
        }

        public async Task<IActionResult> ProfessorCoursesEdit(int? id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (id != user.Id) return RedirectToAction("AccessDenied", "Account");


            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professors.Include(i => i.Courses)
                .ThenInclude(i => i.Course).SingleOrDefaultAsync(m => m.Id == id);
            if (professor == null)
            {
                return NotFound();
            }
            //var listofenr = new List<Enrollment>();
            //foreach (var enrollment in student.Enrollments)
            //{
            //    listofenr.Add(enrollment);
            //}
            PopulateEnrolledCourseData(professor);

            return View(professor);

        }

        [Authorize(Roles = "Professor")]
        [ActionName("CoursesToChoose")]
        public async Task<IActionResult> CoursesToChoose(FormCollection formCollection /*,[System.Web.Http.FromUri] string sem*/)
        {
            string sem = Request.Form["sem"];
            IdentityUser<int> user = await _userManager.FindByNameAsync(User.Identity.Name);
            var requests = await _context.TeachingRequests.Where(id => id.ProfessorID == user.Id && id.SemesterID == int.Parse(sem)).SingleOrDefaultAsync();
            if (requests != null) { return RedirectToAction("ProfessorIndex"); }
            else
            {
                TeachingRequest request = new TeachingRequest()
                {
                    ProfessorID = user.Id,
                };
                if (HttpContext.Session.GetObjectFromJson<TeachingRequest>("request") == null)
                {
                    var semesters = _context.Semesters.OrderBy(m => m.Season).Where(m => m.Open == true).AsNoTracking();
                    ViewBag.Semesters = new SelectList(semesters, "ID", "Season", sem);
                    request.SemesterID = Int32.Parse(sem);
                    //request.SemesterForAssignment = await _context.Semesters.Where(i => i.ID == Int32.Parse(sem)).AsNoTracking().SingleOrDefaultAsync();
                    HttpContext.Session.SetObjectAsJson("request", request);
                }
                else
                {
                    request = HttpContext.Session.GetObjectFromJson<TeachingRequest>("request");
                    var semesters = _context.Semesters.OrderBy(m => m.Season).Where(m => m.Open == true).AsNoTracking();
                    ViewBag.Semesters = new SelectList(semesters, "ID", "Season", request.SemesterID);
                }
                CoursesToChoose courses = new CoursesToChoose();
                var fullCourseList = await _context.Courses.Include(t => t.Department).AsNoTracking().ToListAsync();
                List<ChoosenCourse> somelist = new List<ChoosenCourse>();
                foreach (var s in fullCourseList)
                {
                    ChoosenCourse course = new ChoosenCourse()
                    {
                        SelectedCourses = s,
                        //CourseID = s.CourseID,
                        Checked = false,
                        Choice = (Desire)2,
                    };
                    somelist.Add(course);
                }
                courses.Courses = somelist;


                return View(courses);
            }
        }

        [Authorize(Roles = "Professor")]
        [HttpPost, ActionName("SubmitRequest")]
        public async Task<IActionResult> CoursesToChoose(CoursesToChoose model, FormCollection formCollection)
        {
            foreach (ChoosenCourse item in model.Courses)
            {
                item.SelectedCourses = await _context.Courses.Where(i => i.CourseID == item.SelectedCourses.CourseID).AsNoTracking().SingleOrDefaultAsync();
            }
            if (ModelState.IsValid)
            {
                var request = HttpContext.Session.GetObjectFromJson<TeachingRequest>("request");
                request.ListOfCourses = new List<CoursePreference>();
                foreach (ChoosenCourse item in model.Courses)
                    if (item.Checked == true)
                    {
                        //somelist.Add(item.SelectedCourses);
                        _context.Courses.Attach(item.SelectedCourses);
                        request.ListOfCourses.Add(new CoursePreference { Choice = item.Choice, CourseID = item.SelectedCourses.CourseID, SelectedCourses = item.SelectedCourses });
                    }
                request.ProfessorEntity = await _context.Professors.Where(i => i.Id == request.ProfessorID).SingleOrDefaultAsync();
                _context.Professors.Attach(request.ProfessorEntity);
                //request.ListOfCourses = somelist;
                _context.Add(request);
                await _context.SaveChangesAsync();
            }
   

            return RedirectToAction("ProfessorIndex");
        }

        [Authorize(Roles = "Professor")]
        [ActionName("MyRequest")]
        public async Task<IActionResult> MyRequest(int SemesterID, int ProfessorID)
        {
            IdentityUser<int> user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ProfessorID != user.Id) return RedirectToAction("AccessDenied", "Account");
            else
            {
                TeachingRequest request = await _context.TeachingRequests.Include(i => i.ListOfCourses).ThenInclude(i => i.SelectedCourses).Where(i => i.ProfessorID == ProfessorID && i.SemesterID == SemesterID).AsNoTracking().SingleOrDefaultAsync();
                return View(request);
            }
        }

        private void PopulateEnrolledCourseData(Professor professor)
        {
            var allCourses = _context.Courses;
            if (professor.Courses == null)
                professor.Courses = new List<CourseAssignment>();
            var professorCourses = new HashSet<int>(professor.Courses.Select(c => c.Course.CourseID));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = professorCourses.Contains(course.CourseID),

                });
            }
            ViewData["Courses"] = viewModel;
        }

        public async Task<IActionResult> MyAssignment(int id)
        {
            CourseAssignment assignment = await _context.CourseAssignments.Include(i => i.Course).FirstOrDefaultAsync(i => i.AssignmentID == id);
            //ViewData["Grade"] = new SelectList(_context.Enrollments, "Grade","Grade",enrollment.Grade);
            return View(assignment);
        }
        //// POST: Students/Edit/5
        ////custom method to add/remove MYcourses (student)
        //[HttpPost, ActionName("MyCourse")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> MyCourse(Enrollment myenrollment)
        //{

        //    Enrollment enrollmentToUpdate = await _context.Enrollments.FirstOrDefaultAsync(i => i.EnrollmentID == myenrollment.EnrollmentID);
        //    enrollmentToUpdate.Notes = myenrollment.Notes;
        //    enrollmentToUpdate.Grade = myenrollment.Grade;
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException /* ex */)
        //    {
        //        //Log the error (uncomment ex variable name and write a log.)
        //        ModelState.AddModelError("", "Unable to save changes. " +
        //            "Try again, and if the problem persists, " +
        //            "see your system administrator.");
        //    }
        //    return RedirectToAction("StudentIndex");
        //}
        private Task<IdentityUser<int>> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}