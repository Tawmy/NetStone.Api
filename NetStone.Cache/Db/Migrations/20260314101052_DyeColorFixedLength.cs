using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class DyeColorFixedLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "dye2color",
                table: "character_gears",
                type: "character(6)",
                fixedLength: true,
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "dye1color",
                table: "character_gears",
                type: "character(6)",
                fixedLength: true,
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "dye2color",
                table: "character_gears",
                type: "character varying(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character(6)",
                oldFixedLength: true,
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "dye1color",
                table: "character_gears",
                type: "character varying(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character(6)",
                oldFixedLength: true,
                oldMaxLength: 6,
                oldNullable: true);
        }
    }
}
