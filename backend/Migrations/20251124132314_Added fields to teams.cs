using System.Runtime.InteropServices.Marshalling;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Addedfieldstoteams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FieldId",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "ExtraMinute",
                table: "MatchEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Minute",
                table: "MatchEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_FieldId",
                table: "Teams",
                column: "FieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Fields_FieldId",
                table: "Teams",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Fields_FieldId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_FieldId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "FieldId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ExtraMinute",
                table: "MatchEvents");

            migrationBuilder.DropColumn(
                name: "Minute",
                table: "MatchEvents");
        }
    }
}
