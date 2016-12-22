﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    //Courses Assigned to professor
    public class CourseAssignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssignmentID { get; set; }
        public int ProfessorID { get; set; }
        public int CourseID { get; set; }
        [ForeignKey("ProfessorID")]
        public Professor Professor { get; set; }
        [ForeignKey("CourseID")]
        public Course Course { get; set; }

        [StringLength(50)]
        [Display(Name = "Course Description")]
        public string CourseDescription { get; set; }
        //<--zadel pod files
        //[Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Assignment Date")]
        public DateTime AssignmentDate { get; set; }
        //Current tought course?
        public bool CurrentlyTought { get; set; }
        //Approved or not? (Should probably remove once course request function)
        public bool Approved { get; set; }
    }
}