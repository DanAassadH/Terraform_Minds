using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TerraformMinds.Migrations
{
    public partial class StudentSubmittedFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Role = table.Column<int>(type: "int(1)", nullable: false),
                    EMail = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    Password = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    JoinDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "course",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(type: "int(10)", nullable: false),
                    CourseName = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    Subject = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    CourseDescription = table.Column<string>(type: "varchar(500)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    GradeLevel = table.Column<string>(type: "varchar(20)", nullable: false),
                    CurrentCapacity = table.Column<int>(type: "int(3)", nullable: false),
                    MaxCapacity = table.Column<int>(type: "int(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Course_User",
                        column: x => x.UserID,
                        principalTable: "user",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assignment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CourseID = table.Column<int>(type: "int(10)", nullable: false),
                    Question = table.Column<string>(type: "varchar(500)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    DueDate = table.Column<DateTime>(type: "date", nullable: false),
                    TotalScore = table.Column<int>(type: "int(5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assignment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Assignment_Course",
                        column: x => x.CourseID,
                        principalTable: "course",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resource",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CourseID = table.Column<int>(type: "int(10)", nullable: false),
                    ResourceMaterial = table.Column<string>(type: "varchar(500)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    ResourceUrl = table.Column<string>(type: "varchar(500)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resource", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Resource_Course",
                        column: x => x.CourseID,
                        principalTable: "course",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(type: "int(10)", nullable: false),
                    CourseID = table.Column<int>(type: "int(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Student_Course",
                        column: x => x.CourseID,
                        principalTable: "course",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Student_User",
                        column: x => x.UserID,
                        principalTable: "user",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "submitted",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssignmentID = table.Column<int>(type: "int(10)", nullable: false),
                    StudentID = table.Column<int>(type: "int(10)", nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "date", nullable: false),
                    Answer = table.Column<string>(type: "varchar(2000)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    ScoreObtained = table.Column<int>(type: "int(5)", nullable: true),
                    Remarks = table.Column<string>(type: "varchar(500)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_submitted", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Submit_Assignment",
                        column: x => x.AssignmentID,
                        principalTable: "assignment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submit_Student",
                        column: x => x.StudentID,
                        principalTable: "student",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "ID", "EMail", "FirstName", "JoinDate", "LastName", "Password", "Role" },
                values: new object[,]
                {
                    { -1, "admin.adminson@test.com", "Admin", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adminson", "Test123!", 1 },
                    { -2, "instructor.instructorson@test.com", "Instructor", new DateTime(2020, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Instructorson", "Test123!", 2 },
                    { -3, "student.studentson@test.com", "Student", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Studentson", "Test123!", 3 },
                    { -4, "John.Smith@test.com", "John", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smith", "Test123!", 2 }
                });

            migrationBuilder.InsertData(
                table: "course",
                columns: new[] { "ID", "CourseDescription", "CourseName", "CurrentCapacity", "EndDate", "GradeLevel", "MaxCapacity", "StartDate", "Subject", "UserID" },
                values: new object[,]
                {
                    { -1, "Calculus I: Is an introductory class that will teach the fundamentals of Derviatives, Integrals, Limits and Differential Equations. ", "Calculus I", 10, new DateTime(2019, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Grade 12", 20, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Math", -2 },
                    { -2, "Biology I: ", "Biology I", 10, new DateTime(2019, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Grade 10", 20, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Science", -2 },
                    { -3, "Addition: This course is for students learning addition for their first time. ", "Adding for Beginners", 10, new DateTime(2019, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kindergarten", 20, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Math", -2 },
                    { -4, "Social Studies I: ", "Social Studies I", 20, new DateTime(2019, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Grade 8", 20, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Social Studies", -2 }
                });

            migrationBuilder.InsertData(
                table: "assignment",
                columns: new[] { "ID", "CourseID", "DueDate", "Question", "TotalScore" },
                values: new object[] { -1, -1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assignment: Question Test", 10 });

            migrationBuilder.InsertData(
                table: "resource",
                columns: new[] { "ID", "CourseID", "ResourceMaterial", "ResourceUrl" },
                values: new object[] { -1, -1, "Resource: Material Test", "www.google.ca" });

            migrationBuilder.InsertData(
                table: "student",
                columns: new[] { "ID", "CourseID", "UserID" },
                values: new object[] { -1, -1, -3 });

            migrationBuilder.InsertData(
                table: "submitted",
                columns: new[] { "ID", "Answer", "AssignmentID", "DateSubmitted", "Remarks", "ScoreObtained", "StudentID" },
                values: new object[] { -1, "Submit: Answer Test", -1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Submit: Remarks Test", 6, -1 });

            migrationBuilder.CreateIndex(
                name: "FK_Assignment_Course",
                table: "assignment",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "FK_Course_User",
                table: "course",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "FK_Resource_Course",
                table: "resource",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "FK_Student_Course",
                table: "student",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "FK_Student_User",
                table: "student",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "FK_Submit_Assignment",
                table: "submitted",
                column: "AssignmentID");

            migrationBuilder.CreateIndex(
                name: "FK_Submit_Student",
                table: "submitted",
                column: "StudentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "resource");

            migrationBuilder.DropTable(
                name: "submitted");

            migrationBuilder.DropTable(
                name: "assignment");

            migrationBuilder.DropTable(
                name: "student");

            migrationBuilder.DropTable(
                name: "course");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
