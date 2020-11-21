using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TerraformMinds.Models
{
    [Table("student")]
    public class Student
    {
        [Key]
        [Column(TypeName = "int(10)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "int(10)")]
        public int UserID { get; set; }

        [Column(TypeName = "int(10)")]
        public int CourseID { get; set; }

        [Column(TypeName = "int(10)")]
        public int SubmitID { get; set; }

        //***********************
        // User ForeignKey
        //***********************
        [ForeignKey(nameof(UserID))]
        [InverseProperty(nameof(Models.User.Students))] 
        public virtual User User { get; set; } 

        //***********************
        // Course ForeignKey
        //***********************
        [ForeignKey(nameof(CourseID))]
        [InverseProperty(nameof(Models.Course.Students))] 
        public virtual Course Course { get; set; } 

        //***********************
        // Submit ForeignKey
        //***********************
        [ForeignKey(nameof(SubmitID))]
        [InverseProperty(nameof(Models.Submit.Students))] 
        public virtual Submit Submit { get; set; } 

    }
}
