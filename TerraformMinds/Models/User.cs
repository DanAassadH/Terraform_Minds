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
        public int Role { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string EMail { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }

        [Column(TypeName = "date")]
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
