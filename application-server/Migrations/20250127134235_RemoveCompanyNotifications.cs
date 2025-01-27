using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationServer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCompanyNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_advertisement_company_company_id",
                table: "advertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_application_advertisement_advertisement_id",
                table: "application");

            migrationBuilder.DropForeignKey(
                name: "FK_internship_advertisement_advertisement_id",
                table: "internship");

            migrationBuilder.DropForeignKey(
                name: "FK_internship_company_company_id",
                table: "internship");

            migrationBuilder.DropForeignKey(
                name: "FK_internship_student_student_id",
                table: "internship");

            migrationBuilder.DropTable(
                name: "company_notifications");

            migrationBuilder.AddForeignKey(
                name: "FK_advertisement_company_company_id",
                table: "advertisement",
                column: "company_id",
                principalTable: "company",
                principalColumn: "company_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_application_advertisement_advertisement_id",
                table: "application",
                column: "advertisement_id",
                principalTable: "advertisement",
                principalColumn: "advertisement_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_internship_advertisement_advertisement_id",
                table: "internship",
                column: "advertisement_id",
                principalTable: "advertisement",
                principalColumn: "advertisement_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_internship_company_company_id",
                table: "internship",
                column: "company_id",
                principalTable: "company",
                principalColumn: "company_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_internship_student_student_id",
                table: "internship",
                column: "student_id",
                principalTable: "student",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_advertisement_company_company_id",
                table: "advertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_application_advertisement_advertisement_id",
                table: "application");

            migrationBuilder.DropForeignKey(
                name: "FK_internship_advertisement_advertisement_id",
                table: "internship");

            migrationBuilder.DropForeignKey(
                name: "FK_internship_company_company_id",
                table: "internship");

            migrationBuilder.DropForeignKey(
                name: "FK_internship_student_student_id",
                table: "internship");

            migrationBuilder.CreateTable(
                name: "company_notifications",
                columns: table => new
                {
                    company_notification_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    advertisement_id = table.Column<int>(type: "int", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    student_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_notifications", x => x.company_notification_id);
                    table.ForeignKey(
                        name: "FK_company_notifications_advertisement_advertisement_id",
                        column: x => x.advertisement_id,
                        principalTable: "advertisement",
                        principalColumn: "advertisement_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_company_notifications_company_company_id",
                        column: x => x.company_id,
                        principalTable: "company",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_company_notifications_student_student_id",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_company_notifications_advertisement_id",
                table: "company_notifications",
                column: "advertisement_id");

            migrationBuilder.CreateIndex(
                name: "IX_company_notifications_company_id",
                table: "company_notifications",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_company_notifications_student_id",
                table: "company_notifications",
                column: "student_id");

            migrationBuilder.AddForeignKey(
                name: "FK_advertisement_company_company_id",
                table: "advertisement",
                column: "company_id",
                principalTable: "company",
                principalColumn: "company_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_application_advertisement_advertisement_id",
                table: "application",
                column: "advertisement_id",
                principalTable: "advertisement",
                principalColumn: "advertisement_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_internship_advertisement_advertisement_id",
                table: "internship",
                column: "advertisement_id",
                principalTable: "advertisement",
                principalColumn: "advertisement_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_internship_company_company_id",
                table: "internship",
                column: "company_id",
                principalTable: "company",
                principalColumn: "company_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_internship_student_student_id",
                table: "internship",
                column: "student_id",
                principalTable: "student",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
