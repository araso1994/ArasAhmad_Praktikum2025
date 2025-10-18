using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WuppertalMapMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionAndImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Stops",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Stops",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Stops");
        }
    }
}
