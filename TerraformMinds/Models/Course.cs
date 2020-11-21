﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TerraformMinds.Models
{
    [Table("course")]
    public class Course
    {
        [Key]
        [Column(TypeName = "int(10)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "int(10)")]
        public int UserID { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string CourseName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Subject { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string CourseDescription { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string GradeLevel { get; set; }

        [Column(TypeName = "int(3)")]
        public int CurrentCapacity { get; set; }

        [Column(TypeName = "int(3)")]
        public int MaxCapacity { get; set; }

        //****************
        // User ForeignKey
        //****************
        [ForeignKey(nameof(UserID))]
        [InverseProperty(nameof(Models.User.Courses))]
        public virtual User User { get; set; } 

        //****************
        // Student Link
        //****************
        [InverseProperty(nameof(Student.Course))]
        public virtual ICollection<Student> Students { get; set; } 

        //****************
        // Resource Link
        //****************
        [InverseProperty(nameof(Resource.Course))]
        public virtual ICollection<Resource> Resources { get; set; } 

        //****************
        // Assignment Link
        //****************
        [InverseProperty(nameof(Assignment.Course))]
        public virtual ICollection<Assignment> Assignments { get; set; } 
    }

}
