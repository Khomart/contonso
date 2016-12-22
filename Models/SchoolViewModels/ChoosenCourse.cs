//form for course request
namespace ContosoUniversity.Models.SchoolViewModels
{
    public class ChoosenCourse
    {
        public Course SelectedCourses { get; set; }
        public bool Checked { get; set; }
        public Desire Choice { get; set; }

        //public Semester CourseSemester { get; set; }
    }
}
