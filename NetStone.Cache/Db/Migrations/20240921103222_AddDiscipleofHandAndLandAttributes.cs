using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscipleofHandAndLandAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "healing_magic_potency",
                table: "character_attributes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "attack_magic_potency",
                table: "character_attributes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "control",
                table: "character_attributes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "craftmanship",
                table: "character_attributes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "gathering",
                table: "character_attributes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "perception",
                table: "character_attributes",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "control",
                table: "character_attributes");

            migrationBuilder.DropColumn(
                name: "craftmanship",
                table: "character_attributes");

            migrationBuilder.DropColumn(
                name: "gathering",
                table: "character_attributes");

            migrationBuilder.DropColumn(
                name: "perception",
                table: "character_attributes");

            migrationBuilder.AlterColumn<int>(
                name: "healing_magic_potency",
                table: "character_attributes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "attack_magic_potency",
                table: "character_attributes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
