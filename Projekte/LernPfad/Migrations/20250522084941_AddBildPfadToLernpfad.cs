using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LernPfad.Migrations
{
    /// <inheritdoc />
    public partial class AddBildPfadToLernpfad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BildPfad",
                table: "Lernpfade",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BildPfad",
                table: "Lernpfade");
        }
    }
}
