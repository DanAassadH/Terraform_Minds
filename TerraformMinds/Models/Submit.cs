using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TerraformMinds.Models
{
    [Table("submitted")]
    public class Submit
    {
        [Key]
        [Column(TypeName = "int(10)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "int(10)")]
        public int AssignmentID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateSubmitted { get; set; }

        [Column(TypeName = "varchar(2000)")]
        public string Answer { get; set; }

        [Column(TypeName = "int(5)")]
        public int ScoreObtained { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string Remarks { get; set; }

        //***********************
        // Assignment ForeignKey
        //***********************
        [ForeignKey(nameof(AssignmentID))]
        [InverseProperty(nameof(Models.Assignment.Submissions))]
        public virtual Assignment Assignment { get; set; } 

        //****************
        // Student Link
        //****************
        [InverseProperty(nameof(Student.Submit))]
        public virtual ICollection<Student> Students { get; set; } 
    }
}
