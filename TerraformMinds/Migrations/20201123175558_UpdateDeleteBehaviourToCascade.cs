using Microsoft.EntityFrameworkCore.Migrations;

namespace TerraformMinds.Migrations
{
    public partial class UpdateDeleteBehaviourToCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resource_Course",
                table: "resource");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Course",
                table: "student");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_User",
                table: "student");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
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

            migrationBuilder.AlterColumn<int>(
                name: "ScoreObtained",
                table: "submitted",
                type: "int(5)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int(5)");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "course",
                type: "int(10)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int(10)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Resource_Course",
                table: "resource",
                column: "CourseID",
                principalTable: "course",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Course",
                table: "student",
                column: "CourseID",
                principalTable: "course",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_User",
                table: "student",
                column: "UserID",
                principalTable: "user",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resource_Course",
                table: "resource");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Course",
                table: "student");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_User",
                table: "student");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "user",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "ScoreObtained",
                table: "submitted",
                type: "int(5)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int(5)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "course",
                type: "int(10)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int(10)");

            migrationBuilder.AddForeignKey(
                name: "FK_Resource_Course",
                table: "resource",
                column: "CourseID",
                principalTable: "course",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Course",
                table: "student",
                column: "CourseID",
                principalTable: "course",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_User",
                table: "student",
                column: "UserID",
                principalTable: "user",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
