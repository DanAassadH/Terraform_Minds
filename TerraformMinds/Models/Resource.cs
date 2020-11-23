using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TerraformMinds.Models
{
    [Table("resource")]
    public class Resource
    {
        [Key]
        [Column(TypeName = "int(10)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "int(10)")]
        [Required]
        public int CourseID { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string ResourceMaterial { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string ResourceUrl { get; set; }

        //***********************
        // Course ForeignKey
        //***********************
        [ForeignKey(nameof(CourseID))]
        [InverseProperty(nameof(Models.Course.Resources))] 
        public virtual Course Course { get; set; } 
    }
}
