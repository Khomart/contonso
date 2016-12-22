using ContosoUniversity.Models.UniversityFunctionalityModels;
using ContosoUniversity.UniversityFunctionalityModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public enum Level
    {
        Department, Faculty, University
    }
    public class Committee
    {
        [Key]
        public int CommitteeID { get; set; }
        public int? ProfessorID { get; set; }
        public int? DepartmentID { get; set; }
        public int? FacultyID { get; set; }
        [Required]
        public Level Level { get; set; }

        [ForeignKey("ProfessorID")]
        public Professor Chair { get; set; }
        [ForeignKey("DepartmentID")]
        public Department Department { get; set; }
        [ForeignKey("FacultyID")]
        public Faculty Faculty { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Required]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        public ICollection<CommitieMembership> CommitieMembers { get; set; }
        [Required]
        public Semester Semester { get; set; }
    }
}
