using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationServer.Migrations
{
    /// <inheritdoc />
    public partial class StudentNotificationsTypeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "student_notifications",
                type: "enum('INVITED', 'RECOMMENDED', 'ACCEPTED', 'REJECTED')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "enum('c','r')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "student_notifications",
                type: "enum('c','r')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "enum('INVITED', 'RECOMMENDED', 'ACCEPTED', 'REJECTED')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
