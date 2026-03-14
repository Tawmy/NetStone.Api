using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class Dye : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "dye1color",
                table: "character_gears",
                type: "character varying(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "dye1database_link",
                table: "character_gears",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "dye1name",
                table: "character_gears",
                type: "character varying(63)",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "dye2color",
                table: "character_gears",
                type: "character varying(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "dye2database_link",
                table: "character_gears",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "dye2name",
                table: "character_gears",
                type: "character varying(63)",
                maxLength: 63,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dye1color",
                table: "character_gears");

            migrationBuilder.DropColumn(
                name: "dye1database_link",
                table: "character_gears");

            migrationBuilder.DropColumn(
                name: "dye1name",
                table: "character_gears");

            migrationBuilder.DropColumn(
                name: "dye2color",
                table: "character_gears");

            migrationBuilder.DropColumn(
                name: "dye2database_link",
                table: "character_gears");

            migrationBuilder.DropColumn(
                name: "dye2name",
                table: "character_gears");
        }
    }
}
