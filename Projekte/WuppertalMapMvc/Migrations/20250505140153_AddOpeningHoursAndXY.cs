using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WuppertalMapMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddOpeningHoursAndXY : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StopConnections_Stops_FromStopId",
                table: "StopConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_StopConnections_Stops_ToStopId",
                table: "StopConnections");

            migrationBuilder.DropIndex(
                name: "IX_StopConnections_FromStopId",
                table: "StopConnections");

            migrationBuilder.DropIndex(
                name: "IX_StopConnections_ToStopId",
                table: "StopConnections");

            migrationBuilder.RenameColumn(
                name: "Lines",
                table: "Stops",
                newName: "OpeningHours");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stops",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinesRaw",
                table: "Stops",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "X",
                table: "Stops",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Y",
                table: "Stops",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Line",
                table: "StopConnections",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinesRaw",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "X",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "Line",
                table: "StopConnections");

            migrationBuilder.RenameColumn(
                name: "OpeningHours",
                table: "Stops",
                newName: "Lines");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stops",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_StopConnections_FromStopId",
                table: "StopConnections",
                column: "FromStopId");

            migrationBuilder.CreateIndex(
                name: "IX_StopConnections_ToStopId",
                table: "StopConnections",
                column: "ToStopId");

            migrationBuilder.AddForeignKey(
                name: "FK_StopConnections_Stops_FromStopId",
                table: "StopConnections",
                column: "FromStopId",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StopConnections_Stops_ToStopId",
                table: "StopConnections",
                column: "ToStopId",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
