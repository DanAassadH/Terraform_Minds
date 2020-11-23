using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TerraformMinds.Models
{
    public class LearningManagementContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Resource> Assignments { get; set; }
        public virtual DbSet<Resource> Submissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connection =
                    "server=localhost;" +
                    "port=3306;" +
                    "user=root;" +
                    "database=terraform_minds;";

                string version = "10.4.14-MariaDB";

                optionsBuilder.UseMySql(connection, x => x.ServerVersion(version));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                // Collation
                entity.Property(e => e.EMail)
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Password)
              .HasCharSet("utf8mb4")
              .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.FirstName)
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.LastName)
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_general_ci");

                //SEED DATA
                entity.HasData(
                    new User()
                    {
                        ID = -1,
                        Role = 1,
                        EMail = "admin.adminson@test.com",
                        Password = "Test123!",
                        FirstName = "Admin",
                        LastName = "Adminson",
                        JoinDate = new DateTime(2020, 01, 01)
                    },
                    new User()
                    {
                        ID = -2,
                        Role = 2,
                        EMail = "instructor.instructorson@test.com",
                        Password = "Test123!",
                        FirstName = "Instructor",
                        LastName = "Instructorson",
                        JoinDate = new DateTime(2020, 05, 05)
                    },
                    new User()
                    {
                        ID = -3,
                        Role = 3,
                        EMail = "student.studentson@test.com",
                        Password = "Test123!",
                        FirstName = "Student",
                        LastName = "Studentson",
                        JoinDate = new DateTime(2020, 01, 01)
                    },
                    new User()
                    {
                        ID = -4,
                        Role = 2,
                        EMail = "John.Smith@test.com",
                        Password = "Test123!",
                        FirstName = "John",
                        LastName = "Smith",
                        JoinDate = new DateTime(2020, 01, 01)
                    }
                    );
            });

            modelBuilder.Entity<Course>(entity =>
            {
                string courseKeyName = "FK_" + nameof(Course) +
                    "_" + nameof(User);

                // Collation


                entity.Property(e => e.CourseName)
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Subject)
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.CourseDescription)
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_general_ci");

                entity.HasIndex(e => e.UserID)
                .HasName(courseKeyName);

                entity.HasOne(thisEntity => thisEntity.User)
                .WithMany(parent => parent.Courses)
                .HasForeignKey(thisEntity => thisEntity.UserID)
                .OnDelete(DeleteBehavior.Cascade) // On delete of a course, releated child elements in student and resource tables are removed
                .HasConstraintName(courseKeyName);

                // SEED DATA
                entity.HasData(
                    new Course()
                    {
                        ID = -1,
                        UserID = -2,
                        CourseName = "Calculus I",
                        Subject = "Math",
                        CourseDescription = "Calculus I: Is an introductory class that will teach the fundamentals of Derviatives, Integrals, Limits and Differential Equations. ",
                        GradeLevel = "Grade 12",
                        StartDate = new DateTime(2019, 01, 01),
                        EndDate = new DateTime(2019, 04, 04),
                        CurrentCapacity = 10,
                        MaxCapacity = 20
                    },
                    new Course()
                    {
                        ID = -2,
                        UserID = -2,
                        CourseName = "Biology I",
                        Subject = "Science",
                        CourseDescription = "Biology I: ",
                        GradeLevel = "Grade 10",
                        StartDate = new DateTime(2019, 01, 01),
                        EndDate = new DateTime(2019, 04, 04),
                        CurrentCapacity = 10,
                        MaxCapacity = 20
                    },
                    new Course()
                    {
                        ID = -3,
                        UserID = -2,
                        CourseName = "Adding for Beginners",
                        Subject = "Math",
                        CourseDescription = "Addition: This course is for students learning addition for their first time. ",
                        GradeLevel = "Kindergarten",
                        StartDate = new DateTime(2019, 01, 01),
                        EndDate = new DateTime(2019, 04, 04),
                        CurrentCapacity = 10,
                        MaxCapacity = 20
                    },                    
                    new Course()
                    {
                        ID = -4,
                        UserID = -2,
                        CourseName = "Social Studies I",
                        Subject = "Social Studies",
                        CourseDescription = "Social Studies I: ",
                        GradeLevel = "Grade 8",
                        StartDate = new DateTime(2019, 01, 01),
                        EndDate = new DateTime(2019, 04, 04),
                        CurrentCapacity = 20,
                        MaxCapacity = 20
                    }
                    );
            });

            modelBuilder.Entity<Student>(entity =>
            {
                string studentUserKeyName = "FK_" + nameof(Student) +
                    "_" + nameof(User);

                entity.HasIndex(e => e.UserID)
                .HasName(studentUserKeyName);

                entity.HasOne(thisEntity => thisEntity.User)
                .WithMany(parent => parent.Students)
                .HasForeignKey(thisEntity => thisEntity.UserID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName(studentUserKeyName);

                string studentCourseKeyName = "FK_" + nameof(Student) +
                    "_" + nameof(Course);

                entity.HasIndex(e => e.CourseID)
                .HasName(studentCourseKeyName);

                entity.HasOne(thisEntity => thisEntity.Course)
                .WithMany(parent => parent.Students)
                .HasForeignKey(thisEntity => thisEntity.CourseID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName(studentCourseKeyName);

                entity.HasData(
                   new Student()
                   {
                       ID = -1,
                       CourseID = -1,
                       UserID = -3/*,
                       SubmitID = -1*/
                   });
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                string resourceKeyName = "FK_" + nameof(Resource) +
                    "_" + nameof(Course);

                // Collation
                entity.Property(e => e.ResourceMaterial)
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.ResourceUrl)
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_general_ci");

                entity.HasIndex(e => e.CourseID)
                .HasName(resourceKeyName);

                entity.HasOne(thisEntity => thisEntity.Course)
                .WithMany(parent => parent.Resources)
                .HasForeignKey(thisEntity => thisEntity.CourseID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName(resourceKeyName);

                entity.HasData(
                   new Resource()
                   {
                       ID = -1,
                       CourseID = -1,
                       ResourceMaterial = "Resource: Material Test",
                       ResourceUrl = "www.google.ca"
                   });
            });

            modelBuilder.Entity<Assignment>(entity =>
            {
                string assignmentKeyName = "FK_" + nameof(Assignment) +
                    "_" + nameof(Course);

                // Collation
                entity.Property(e => e.Question)
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_general_ci");

                entity.HasIndex(e => e.CourseID)
                .HasName(assignmentKeyName);

                entity.HasOne(thisEntity => thisEntity.Course)
                .WithMany(parent => parent.Assignments)
                .HasForeignKey(thisEntity => thisEntity.CourseID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName(assignmentKeyName);

                entity.HasData(
                   new Assignment()
                   {
                       ID = -1,
                       CourseID = -1,
                       Question = "Assignment: Question Test",
                       TotalScore = 10
                   });
            });

            modelBuilder.Entity<Submit>(entity =>
            {
                string submitKeyName = "FK_" + nameof(Submit) +
                    "_" + nameof(Assignment);

                string submitKeyStudent = "FK_" + nameof(Submit) +
                    "_" + nameof(Student);

                // Collation
                entity.Property(e => e.Answer)
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Remarks)
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_general_ci");

                entity.HasIndex(e => e.AssignmentID)
                .HasName(submitKeyName);

                entity.HasIndex(e => e.StudentID)
               .HasName(submitKeyStudent);

                entity.HasOne(thisEntity => thisEntity.Assignment)
                .WithMany(parent => parent.Submissions)
                .HasForeignKey(thisEntity => thisEntity.AssignmentID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName(submitKeyName);

                entity.HasOne(thisEntity => thisEntity.Student)
                .WithMany(parent => parent.Submissions)
                .HasForeignKey(thisEntity => thisEntity.StudentID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName(submitKeyStudent);

                entity.HasData(
                   new Submit()
                   {
                       ID = -1,
                       AssignmentID = -1,
                       StudentID = -1,
                       Answer = "Submit: Answer Test",
                       ScoreObtained = 6,
                       Remarks = "Submit: Remarks Test"
                   });
            });
        }
    }
}
