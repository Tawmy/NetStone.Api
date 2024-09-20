using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddItemLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "item_level",
                table: "character_gears",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "item_level",
                table: "character_gears");
        }
    }
}
