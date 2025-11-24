using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class StatusInsteadofisPlayed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPlayed",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Matches");

            migrationBuilder.AddColumn<bool>(
                name: "isPlayed",
                table: "Matches",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
