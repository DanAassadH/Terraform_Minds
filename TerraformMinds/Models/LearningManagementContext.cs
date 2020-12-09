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
        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<Submit> Submissions { get; set; }

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

            });
        }
    }
}
