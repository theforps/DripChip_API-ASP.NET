using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DripChipAPI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class editModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalLocationInfo");

            migrationBuilder.DropTable(
                name: "AnimalTypes");

            migrationBuilder.AddColumn<long>(
                name: "Animalid",
                table: "Types",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Animalid",
                table: "LocationInfo",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Types_Animalid",
                table: "Types",
                column: "Animalid");

            migrationBuilder.CreateIndex(
                name: "IX_LocationInfo_Animalid",
                table: "LocationInfo",
                column: "Animalid");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationInfo_Animals_Animalid",
                table: "LocationInfo",
                column: "Animalid",
                principalTable: "Animals",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Types_Animals_Animalid",
                table: "Types",
                column: "Animalid",
                principalTable: "Animals",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationInfo_Animals_Animalid",
                table: "LocationInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_Types_Animals_Animalid",
                table: "Types");

            migrationBuilder.DropIndex(
                name: "IX_Types_Animalid",
                table: "Types");

            migrationBuilder.DropIndex(
                name: "IX_LocationInfo_Animalid",
                table: "LocationInfo");

            migrationBuilder.DropColumn(
                name: "Animalid",
                table: "Types");

            migrationBuilder.DropColumn(
                name: "Animalid",
                table: "LocationInfo");

            migrationBuilder.CreateTable(
                name: "AnimalLocationInfo",
                columns: table => new
                {
                    Animalsid = table.Column<long>(type: "INTEGER", nullable: false),
                    visitedLocationsid = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalLocationInfo", x => new { x.Animalsid, x.visitedLocationsid });
                    table.ForeignKey(
                        name: "FK_AnimalLocationInfo_Animals_Animalsid",
                        column: x => x.Animalsid,
                        principalTable: "Animals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalLocationInfo_LocationInfo_visitedLocationsid",
                        column: x => x.visitedLocationsid,
                        principalTable: "LocationInfo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalTypes",
                columns: table => new
                {
                    Animalsid = table.Column<long>(type: "INTEGER", nullable: false),
                    animalTypesid = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalTypes", x => new { x.Animalsid, x.animalTypesid });
                    table.ForeignKey(
                        name: "FK_AnimalTypes_Animals_Animalsid",
                        column: x => x.Animalsid,
                        principalTable: "Animals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalTypes_Types_animalTypesid",
                        column: x => x.animalTypesid,
                        principalTable: "Types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalLocationInfo_visitedLocationsid",
                table: "AnimalLocationInfo",
                column: "visitedLocationsid");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalTypes_animalTypesid",
                table: "AnimalTypes",
                column: "animalTypesid");
        }
    }
}
