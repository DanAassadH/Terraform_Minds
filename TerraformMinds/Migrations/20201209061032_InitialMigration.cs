using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TerraformMinds.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "resource");

            migrationBuilder.DeleteData(
                table: "course",
                keyColumn: "ID",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "course",
                keyColumn: "ID",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "course",
                keyColumn: "ID",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "submitted",
                keyColumn: "ID",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "user",
                keyColumn: "ID",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "user",
                keyColumn: "ID",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "assignment",
                keyColumn: "ID",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "student",
                keyColumn: "ID",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "course",
                keyColumn: "ID",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "user",
                keyColumn: "ID",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "user",
                keyColumn: "ID",
                keyValue: -2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "FK_Resource_Course",
                table: "resource",
                column: "CourseID");
        }
    }
}
