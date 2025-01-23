using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationServer.Migrations
{
    /// <inheritdoc />
    public partial class SplitFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feedback");

            migrationBuilder.CreateTable(
                name: "company_feedbacks",
                columns: table => new
                {
                    internship_id = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_feedbacks", x => x.internship_id);
                    table.ForeignKey(
                        name: "FK_company_feedbacks_internship_internship_id",
                        column: x => x.internship_id,
                        principalTable: "internship",
                        principalColumn: "internship_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "student_feedbacks",
                columns: table => new
                {
                    internship_id = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_feedbacks", x => x.internship_id);
                    table.ForeignKey(
                        name: "FK_student_feedbacks_internship_internship_id",
                        column: x => x.internship_id,
                        principalTable: "internship",
                        principalColumn: "internship_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "company_feedbacks");

            migrationBuilder.DropTable(
                name: "student_feedbacks");

            migrationBuilder.CreateTable(
                name: "feedback",
                columns: table => new
                {
                    internship_id = table.Column<int>(type: "int", nullable: false),
                    company_comment = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    company_rating = table.Column<int>(type: "int", nullable: false),
                    student_comment = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    student_rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedback", x => x.internship_id);
                    table.ForeignKey(
                        name: "FK_feedback_internship_internship_id",
                        column: x => x.internship_id,
                        principalTable: "internship",
                        principalColumn: "internship_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
