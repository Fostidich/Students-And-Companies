using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationServer.Migrations
{
    /// <inheritdoc />
    public partial class smallfixfulldatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "internship_id",
                table: "student");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "internship_id",
                table: "student",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
