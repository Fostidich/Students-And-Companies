using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationServer.Migrations
{
    /// <inheritdoc />
    public partial class SmallChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_application_student_student_id",
                table: "application");

            migrationBuilder.DropColumn(
                name: "proposed_start",
                table: "application");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "application",
                type: "enum('PENDING', 'ACCEPTED', 'REJECTED')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "enum('REQUESTED', 'ACCEPTED', 'STARTED')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "advertisement",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_application_student_student_id",
                table: "application",
                column: "student_id",
                principalTable: "student",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_application_student_student_id",
                table: "application");

            migrationBuilder.DropColumn(
                name: "name",
                table: "advertisement");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "application",
                type: "enum('REQUESTED', 'ACCEPTED', 'STARTED')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "enum('PENDING', 'ACCEPTED', 'REJECTED')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "proposed_start",
                table: "application",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_application_student_student_id",
                table: "application",
                column: "student_id",
                principalTable: "student",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
