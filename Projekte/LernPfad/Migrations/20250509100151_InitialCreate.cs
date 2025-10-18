using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LernPfad.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lernpfade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Beschreibung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutorId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lernpfade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lernschritte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Inhalt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reihenfolge = table.Column<int>(type: "int", nullable: false),
                    LernpfadId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lernschritte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lernschritte_Lernpfade_LernpfadId",
                        column: x => x.LernpfadId,
                        principalTable: "Lernpfade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lernschritte_LernpfadId",
                table: "Lernschritte",
                column: "LernpfadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lernschritte");

            migrationBuilder.DropTable(
                name: "Lernpfade");
        }
    }
}
