using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models.SchoolViewModels;
using ContosoUniversity.UniversityFunctionalityModels.Models;

namespace ContosoUniversity.Models
{
    public enum Desire
    {
        [Display(Name = "Most Preferable")]
        desired = 3,
        [Display(Name = "Preferable")]
        prefered = 2,
        [Display(Name = "Satisfying")]
        satisfying = 1,
    }
    public class TeachingRequest
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SemesterID { set; get; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProfessorID { set; get; }
        public ICollection<CoursePreference> ListOfCourses { set; get; }

        [ForeignKey("SemesterID")]
        public Semester SemesterForAssignment { set; get; }
        [ForeignKey("ProfessorID")]
        public Professor ProfessorEntity { set; get; }

        public string Annotation { set; get; } //Custom text field with request...
        public bool Approved { get; set; }
    }
}