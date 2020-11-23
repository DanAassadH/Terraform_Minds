using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TerraformMinds.Models
{
    [Table("assignment")]
    public class Assignment
    {
        [Key]
        [Column(TypeName = "int(10)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "int(10)")]
        public int CourseID { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string Question { get; set; }

        [Column(TypeName = "date")]
        [Required]
        public DateTime DueDate { get; set; }

        [Column(TypeName = "int(5)")]
        public int TotalScore { get; set; }

        //***********************
        // Course ForeignKey
        //***********************
        [ForeignKey(nameof(CourseID))]
        [InverseProperty(nameof(Models.Course.Assignments))] 
        public virtual Course Course { get; set; }

        //****************
        // Submit Link
        //****************
        [InverseProperty(nameof(Submit.Assignment))]
        public virtual ICollection<Submit> Submissions { get; set; }

    }
}
