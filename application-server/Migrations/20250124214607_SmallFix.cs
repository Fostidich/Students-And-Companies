using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationServer.Migrations
{
    /// <inheritdoc />
    public partial class SmallFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_advertisement_skills_advertisement_advertisement_id",
                table: "advertisement_skills");

            migrationBuilder.DropForeignKey(
                name: "FK_student_skills_student_student_id",
                table: "student_skills");

            migrationBuilder.AddForeignKey(
                name: "FK_advertisement_skills_advertisement_advertisement_id",
                table: "advertisement_skills",
                column: "advertisement_id",
                principalTable: "advertisement",
                principalColumn: "advertisement_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_student_skills_student_student_id",
                table: "student_skills",
                column: "student_id",
                principalTable: "student",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_advertisement_skills_advertisement_advertisement_id",
                table: "advertisement_skills");

            migrationBuilder.DropForeignKey(
                name: "FK_student_skills_student_student_id",
                table: "student_skills");

            migrationBuilder.AddForeignKey(
                name: "FK_advertisement_skills_advertisement_advertisement_id",
                table: "advertisement_skills",
                column: "advertisement_id",
                principalTable: "advertisement",
                principalColumn: "advertisement_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_student_skills_student_student_id",
                table: "student_skills",
                column: "student_id",
                principalTable: "student",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
