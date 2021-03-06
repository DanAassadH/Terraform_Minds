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

                    b.Property<int>("UserID")
                        .HasColumnType("int(10)");

                    b.HasKey("ID");

                    b.HasIndex("UserID")
                        .HasName("FK_Course_User");

                    b.ToTable("course");
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

                    b.Property<int?>("ScoreObtained")
                        .HasColumnType("int(5)");

                    b.Property<int>("StudentID")
                        .HasColumnType("int(10)");

                    b.HasKey("ID");

                    b.HasIndex("AssignmentID")
                        .HasName("FK_Submit_Assignment");

                    b.HasIndex("StudentID")
                        .HasName("FK_Submit_Student");

                    b.ToTable("submitted");
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
                        .IsRequired()
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
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TerraformMinds.Models.Student", b =>
                {
                    b.HasOne("TerraformMinds.Models.Course", "Course")
                        .WithMany("Students")
                        .HasForeignKey("CourseID")
                        .HasConstraintName("FK_Student_Course")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TerraformMinds.Models.User", "User")
                        .WithMany("Students")
                        .HasForeignKey("UserID")
                        .HasConstraintName("FK_Student_User")
                        .OnDelete(DeleteBehavior.Cascade)
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
