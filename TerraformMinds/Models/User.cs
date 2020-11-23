using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TerraformMinds.Models
{
    [Table("user")]
    public class User
    {
        public User()
        {
            Courses = new HashSet<Course>();
        }

        [Key]
        [Column(TypeName = "int(10)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "int(1)")]
        [Required]
        public int Role { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        [Required]
        public string EMail { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Password { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        [Required]
        public string Password { get; set; }
        
        [Column(TypeName = "varchar(50)")]
        [Required]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }

        [Column(TypeName = "date")]
        [Required]
        public DateTime JoinDate { get; set; }

        //****************
        // Course Link
        //****************
        [InverseProperty(nameof(Models.Course.User))]
        public virtual ICollection<Course> Courses { get; set; }

        //****************
        // Student Link
        //****************
        [InverseProperty(nameof(Models.Student.User))]
        public virtual ICollection<Student> Students { get; set; }



    }
}
