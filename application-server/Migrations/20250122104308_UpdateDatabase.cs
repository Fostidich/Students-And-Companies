using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "company_notifications",
                columns: table => new
                {
                    company_notification_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    student_id = table.Column<int>(type: "int", nullable: false),
                    advertisement_id = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "enum('a','r')", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
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

            migrationBuilder.CreateTable(
                name: "student_notifications",
                columns: table => new
                {
                    student_notification_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    student_id = table.Column<int>(type: "int", nullable: false),
                    advertisement_id = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "enum('c','r')", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_notifications", x => x.student_notification_id);
                    table.ForeignKey(
                        name: "FK_student_notifications_advertisement_advertisement_id",
                        column: x => x.advertisement_id,
                        principalTable: "advertisement",
                        principalColumn: "advertisement_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_notifications_student_student_id",
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

            migrationBuilder.CreateIndex(
                name: "IX_student_notifications_advertisement_id",
                table: "student_notifications",
                column: "advertisement_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_notifications_student_id",
                table: "student_notifications",
                column: "student_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "company_notifications");

            migrationBuilder.DropTable(
                name: "student_notifications");
        }
    }
}
