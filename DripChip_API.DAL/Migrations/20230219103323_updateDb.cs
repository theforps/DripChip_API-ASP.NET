using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DripChipAPI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Types_Animals_Animalid",
                table: "Types");

            migrationBuilder.DropIndex(
                name: "IX_Types_Animalid",
                table: "Types");

            migrationBuilder.DropColumn(
                name: "Animalid",
                table: "Types");

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
                name: "IX_AnimalTypes_animalTypesid",
                table: "AnimalTypes",
                column: "animalTypesid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalTypes");

            migrationBuilder.AddColumn<long>(
                name: "Animalid",
                table: "Types",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Types_Animalid",
                table: "Types",
                column: "Animalid");

            migrationBuilder.AddForeignKey(
                name: "FK_Types_Animals_Animalid",
                table: "Types",
                column: "Animalid",
                principalTable: "Animals",
                principalColumn: "id");
        }
    }
}
