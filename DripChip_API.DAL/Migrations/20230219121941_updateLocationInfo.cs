using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DripChipAPI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateLocationInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationInfo_Locations_locationPointIdid",
                table: "LocationInfo");

            migrationBuilder.RenameColumn(
                name: "locationPointIdid",
                table: "LocationInfo",
                newName: "locationPointid");

            migrationBuilder.RenameIndex(
                name: "IX_LocationInfo_locationPointIdid",
                table: "LocationInfo",
                newName: "IX_LocationInfo_locationPointid");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationInfo_Locations_locationPointid",
                table: "LocationInfo",
                column: "locationPointid",
                principalTable: "Locations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationInfo_Locations_locationPointid",
                table: "LocationInfo");

            migrationBuilder.RenameColumn(
                name: "locationPointid",
                table: "LocationInfo",
                newName: "locationPointIdid");

            migrationBuilder.RenameIndex(
                name: "IX_LocationInfo_locationPointid",
                table: "LocationInfo",
                newName: "IX_LocationInfo_locationPointIdid");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationInfo_Locations_locationPointIdid",
                table: "LocationInfo",
                column: "locationPointIdid",
                principalTable: "Locations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
