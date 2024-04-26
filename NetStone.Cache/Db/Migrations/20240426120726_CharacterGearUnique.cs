using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class CharacterGearUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_character_gears_character_id",
                table: "character_gears");

            migrationBuilder.CreateIndex(
                name: "ix_character_gears_character_id_slot",
                table: "character_gears",
                columns: new[] { "character_id", "slot" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_character_gears_character_id_slot",
                table: "character_gears");

            migrationBuilder.CreateIndex(
                name: "ix_character_gears_character_id",
                table: "character_gears",
                column: "character_id");
        }
    }
}
