using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class CoursePreference
    {
        public int RequestID { get; set; }
        public int CourseID { get; set; }
        public TeachingRequest Request { get; set; }
        public Course SelectedCourses { get; set; }

        public Desire Choice { get; set; }
    }
}
