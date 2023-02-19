using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DripChipAPI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    weight = table.Column<float>(type: "REAL", nullable: false),
                    length = table.Column<float>(type: "REAL", nullable: false),
                    height = table.Column<float>(type: "REAL", nullable: false),
                    gender = table.Column<string>(type: "TEXT", nullable: false),
                    lifeStatus = table.Column<string>(type: "TEXT", nullable: false),
                    chippingDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    chipperId = table.Column<int>(type: "INTEGER", nullable: false),
                    chippingLocationId = table.Column<long>(type: "INTEGER", nullable: false),
                    deathDateTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    latitude = table.Column<double>(type: "REAL", nullable: false),
                    longitude = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    firstName = table.Column<string>(type: "TEXT", nullable: false),
                    lastName = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    type = table.Column<string>(type: "TEXT", nullable: false),
                    Animalid = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.id);
                    table.ForeignKey(
                        name: "FK_Types_Animals_Animalid",
                        column: x => x.Animalid,
                        principalTable: "Animals",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LocationInfo",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    dateTimeOfVisitLocationPoint = table.Column<DateTime>(type: "TEXT", nullable: false),
                    locationPointIdid = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationInfo", x => x.id);
                    table.ForeignKey(
                        name: "FK_LocationInfo_Locations_locationPointIdid",
                        column: x => x.locationPointIdid,
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AnimalLocationInfo_visitedLocationsid",
                table: "AnimalLocationInfo",
                column: "visitedLocationsid");

            migrationBuilder.CreateIndex(
                name: "IX_LocationInfo_locationPointIdid",
                table: "LocationInfo",
                column: "locationPointIdid");

            migrationBuilder.CreateIndex(
                name: "IX_Types_Animalid",
                table: "Types",
                column: "Animalid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalLocationInfo");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "LocationInfo");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
