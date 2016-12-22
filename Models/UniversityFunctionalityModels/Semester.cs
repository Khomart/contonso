using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.UniversityFunctionalityModels.Models
{
    public enum Term
    {
        [Display(Name = "Autumn")]
        autumn,
        [Display(Name = "Winter")]
        winter,
        [Display(Name = "Summer First")]
        summer1,
        [Display(Name = "Summer Second")]
        summer2
    }
    public class Semester
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Term Start")]
        public DateTime StartingDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Term End")]
        public DateTime EndDate { get; set; }

        public Term Season { set; get; }

        public bool Open { set; get; }
        public List<CourseAssignment> AssignedCourses { set; get; }
        public List<Committee> CommitiesRunning { set; get; }
        public List<Enrollment> EnrollmentsInCourses { set; get; }

        public string Title
        {
            get
            {
                return Season + " of " + StartingDate.Year;
            }
        }
    }
}
