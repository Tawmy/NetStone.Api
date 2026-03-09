using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class IconUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "glamour_icon_link",
                table: "character_gears",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "item_icon_link",
                table: "character_gears",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "glamour_icon_link",
                table: "character_gears");

            migrationBuilder.DropColumn(
                name: "item_icon_link",
                table: "character_gears");
        }
    }
}
