using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class FcReputation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "immortal_flames_progress",
                table: "free_companies",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "immortal_flames_rank",
                table: "free_companies",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "maelstrom_progress",
                table: "free_companies",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "maelstrom_rank",
                table: "free_companies",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "twin_adder_progress",
                table: "free_companies",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "twin_adder_rank",
                table: "free_companies",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "immortal_flames_progress",
                table: "free_companies");

            migrationBuilder.DropColumn(
                name: "immortal_flames_rank",
                table: "free_companies");

            migrationBuilder.DropColumn(
                name: "maelstrom_progress",
                table: "free_companies");

            migrationBuilder.DropColumn(
                name: "maelstrom_rank",
                table: "free_companies");

            migrationBuilder.DropColumn(
                name: "twin_adder_progress",
                table: "free_companies");

            migrationBuilder.DropColumn(
                name: "twin_adder_rank",
                table: "free_companies");
        }
    }
}
