using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TerraformMinds.Migrations
{
    public partial class UpdateNullFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_submitted_SubmitID",
                table: "student");

            migrationBuilder.DropIndex(
                name: "IX_student_SubmitID",
                table: "student");

            migrationBuilder.DropColumn(
                name: "SubmitID",
                table: "student");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "user",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "EMail",
                table: "user",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "user",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateSubmitted",
                table: "submitted",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "submitted",
                type: "varchar(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "StudentID",
                table: "submitted",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "course",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "GradeLevel",
                table: "course",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CourseName",
                table: "course",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "CourseDescription",
                table: "course",
                type: "varchar(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "assignment",
                type: "varchar(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "assignment",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "assignment",
                keyColumn: "ID",
                keyValue: -1,
                column: "DueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "submitted",
                keyColumn: "ID",
                keyValue: -1,
                columns: new[] { "DateSubmitted", "StudentID" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), -1 });

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "ID",
                keyValue: -3,
                column: "Password",
                value: "student");

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "ID",
                keyValue: -2,
                column: "Password",
                value: "1234");

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "ID",
                keyValue: -1,
                column: "Password",
                value: "admin");

            migrationBuilder.CreateIndex(
                name: "FK_Submit_Student",
                table: "submitted",
                column: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Submit_Student",
                table: "submitted",
                column: "StudentID",
                principalTable: "student",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submit_Student",
                table: "submitted");

            migrationBuilder.DropIndex(
                name: "FK_Submit_Student",
                table: "submitted");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "user");

            migrationBuilder.DropColumn(
                name: "StudentID",
                table: "submitted");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "user",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "EMail",
                table: "user",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateSubmitted",
                table: "submitted",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "submitted",
                type: "varchar(2000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(2000)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "SubmitID",
                table: "student",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "course",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "GradeLevel",
                table: "course",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "CourseName",
                table: "course",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "CourseDescription",
                table: "course",
                type: "varchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "assignment",
                type: "varchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "assignment",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.UpdateData(
                table: "assignment",
                keyColumn: "ID",
                keyValue: -1,
                column: "DueDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "student",
                keyColumn: "ID",
                keyValue: -1,
                column: "SubmitID",
                value: -1);

            migrationBuilder.UpdateData(
                table: "submitted",
                keyColumn: "ID",
                keyValue: -1,
                column: "DateSubmitted",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_student_SubmitID",
                table: "student",
                column: "SubmitID");

            migrationBuilder.AddForeignKey(
                name: "FK_student_submitted_SubmitID",
                table: "student",
                column: "SubmitID",
                principalTable: "submitted",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
