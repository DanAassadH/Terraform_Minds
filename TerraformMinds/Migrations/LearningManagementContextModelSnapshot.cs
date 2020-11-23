﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TerraformMinds.Models;

namespace TerraformMinds.Migrations
{
    [DbContext(typeof(LearningManagementContext))]
    partial class LearningManagementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TerraformMinds.Models.Assignment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(10)");

                    b.Property<int>("CourseID")
                        .HasColumnType("int(10)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("date");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<int>("TotalScore")
                        .HasColumnType("int(5)");

                    b.HasKey("ID");

                    b.HasIndex("CourseID")
                        .HasName("FK_Assignment_Course");

                    b.ToTable("assignment");

                    b.HasData(
                        new
                        {
                            ID = -1,
                            CourseID = -1,
                            DueDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Question = "Assignment: Question Test",
                            TotalScore = 10
                        });
                });

            modelBuilder.Entity("TerraformMinds.Models.Course", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(10)");

                    b.Property<string>("CourseDescription")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<int>("CurrentCapacity")
                        .HasColumnType("int(3)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("date");

                    b.Property<string>("GradeLevel")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int>("MaxCapacity")
                        .HasColumnType("int(3)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("date");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<int?>("UserID")
                        .HasColumnType("int(10)");

                    b.HasKey("ID");

                    b.HasIndex("UserID")
                        .HasName("FK_Course_User");

                    b.ToTable("course");

                    b.HasData(
                        new
                        {
                            ID = -1,
                            CourseDescription = "Calculus I: Is an introductory class that will teach the fundamentals of Derviatives, Integrals, Limits and Differential Equations. ",
                            CourseName = "Calculus I",
                            CurrentCapacity = 10,
                            EndDate = new DateTime(2019, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GradeLevel = "Grade 12",
                            MaxCapacity = 20,
                            StartDate = new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Subject = "Math",
                            UserID = -2
                        },
                        new
                        {
                            ID = -2,
                            CourseDescription = "Biology I: ",
                            CourseName = "Biology I",
                            CurrentCapacity = 10,
                            EndDate = new DateTime(2019, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GradeLevel = "Grade 10",
                            MaxCapacity = 20,
                            StartDate = new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Subject = "Science",
                            UserID = -2
                        },
                        new
                        {
                            ID = -3,
                            CourseDescription = "Addition: This course is for students learning addition for their first time. ",
                            CourseName = "Adding for Beginners",
                            CurrentCapacity = 10,
                            EndDate = new DateTime(2019, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GradeLevel = "Kindergarten",
                            MaxCapacity = 20,
                            StartDate = new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Subject = "Math",
                            UserID = -2
                        },
                        new
                        {
                            ID = -4,
                            CourseDescription = "Social Studies I: ",
                            CourseName = "Social Studies I",
                            CurrentCapacity = 20,
                            EndDate = new DateTime(2019, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GradeLevel = "Grade 8",
                            MaxCapacity = 20,
                            StartDate = new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Subject = "Social Studies",
                            UserID = -2
                        });
                });

            modelBuilder.Entity("TerraformMinds.Models.Resource", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(10)");

                    b.Property<int>("CourseID")
                        .HasColumnType("int(10)");

                    b.Property<string>("ResourceMaterial")
                        .HasColumnType("varchar(500)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<string>("ResourceUrl")
                        .HasColumnType("varchar(500)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.HasKey("ID");

                    b.HasIndex("CourseID")
                        .HasName("FK_Resource_Course");

                    b.ToTable("resource");

                    b.HasData(
                        new
                        {
                            ID = -1,
                            CourseID = -1,
                            ResourceMaterial = "Resource: Material Test",
                            ResourceUrl = "www.google.ca"
                        });
                });

            modelBuilder.Entity("TerraformMinds.Models.Student", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(10)");

                    b.Property<int>("CourseID")
                        .HasColumnType("int(10)");

                    b.Property<int>("UserID")
                        .HasColumnType("int(10)");

                    b.HasKey("ID");

                    b.HasIndex("CourseID")
                        .HasName("FK_Student_Course");

                    b.HasIndex("UserID")
                        .HasName("FK_Student_User");

                    b.ToTable("student");

                    b.HasData(
                        new
                        {
                            ID = -1,
                            CourseID = -1,
                            UserID = -3
                        });
                });

            modelBuilder.Entity("TerraformMinds.Models.Submit", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(10)");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("varchar(2000)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<int>("AssignmentID")
                        .HasColumnType("int(10)");

                    b.Property<DateTime>("DateSubmitted")
                        .HasColumnType("date");

                    b.Property<string>("Remarks")
                        .HasColumnType("varchar(500)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<int>("ScoreObtained")
                        .HasColumnType("int(5)");

                    b.Property<int>("StudentID")
                        .HasColumnType("int(10)");

                    b.HasKey("ID");

                    b.HasIndex("AssignmentID")
                        .HasName("FK_Submit_Assignment");

                    b.HasIndex("StudentID")
                        .HasName("FK_Submit_Student");

                    b.ToTable("submitted");

                    b.HasData(
                        new
                        {
                            ID = -1,
                            Answer = "Submit: Answer Test",
                            AssignmentID = -1,
                            DateSubmitted = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Remarks = "Submit: Remarks Test",
                            ScoreObtained = 6,
                            StudentID = -1
                        });
                });

            modelBuilder.Entity("TerraformMinds.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(10)");

                    b.Property<string>("EMail")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("date");

                    b.Property<string>("LastName")
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<int>("Role")
                        .HasColumnType("int(1)");

                    b.HasKey("ID");

                    b.ToTable("user");

                    b.HasData(
                        new
                        {
                            ID = -1,
                            EMail = "admin.adminson@test.com",
                            FirstName = "Admin",
                            JoinDate = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Adminson",
                            Password = "Test123!",
                            Role = 1
                        },
                        new
                        {
                            ID = -2,
                            EMail = "instructor.instructorson@test.com",
                            FirstName = "Instructor",
                            JoinDate = new DateTime(2020, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Instructorson",
                            Password = "Test123!",
                            Role = 2
                        },
                        new
                        {
                            ID = -3,
                            EMail = "student.studentson@test.com",
                            FirstName = "Student",
                            JoinDate = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Studentson",
                            Password = "Test123!",
                            Role = 3
                        },
                        new
                        {
                            ID = -4,
                            EMail = "John.Smith@test.com",
                            FirstName = "John",
                            JoinDate = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Smith",
                            Password = "Test123!",
                            Role = 2
                        });
                });

            modelBuilder.Entity("TerraformMinds.Models.Assignment", b =>
                {
                    b.HasOne("TerraformMinds.Models.Course", "Course")
                        .WithMany("Assignments")
                        .HasForeignKey("CourseID")
                        .HasConstraintName("FK_Assignment_Course")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TerraformMinds.Models.Course", b =>
                {
                    b.HasOne("TerraformMinds.Models.User", "User")
                        .WithMany("Courses")
                        .HasForeignKey("UserID")
                        .HasConstraintName("FK_Course_User")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TerraformMinds.Models.Resource", b =>
                {
                    b.HasOne("TerraformMinds.Models.Course", "Course")
                        .WithMany("Resources")
                        .HasForeignKey("CourseID")
                        .HasConstraintName("FK_Resource_Course")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("TerraformMinds.Models.Student", b =>
                {
                    b.HasOne("TerraformMinds.Models.Course", "Course")
                        .WithMany("Students")
                        .HasForeignKey("CourseID")
                        .HasConstraintName("FK_Student_Course")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TerraformMinds.Models.User", "User")
                        .WithMany("Students")
                        .HasForeignKey("UserID")
                        .HasConstraintName("FK_Student_User")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("TerraformMinds.Models.Submit", b =>
                {
                    b.HasOne("TerraformMinds.Models.Assignment", "Assignment")
                        .WithMany("Submissions")
                        .HasForeignKey("AssignmentID")
                        .HasConstraintName("FK_Submit_Assignment")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TerraformMinds.Models.Student", "Student")
                        .WithMany("Submissions")
                        .HasForeignKey("StudentID")
                        .HasConstraintName("FK_Submit_Student")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
