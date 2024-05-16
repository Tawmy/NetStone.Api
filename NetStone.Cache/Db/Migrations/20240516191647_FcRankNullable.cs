using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class FcRankNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "rank_icon",
                table: "free_company_members",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "rank",
                table: "free_company_members",
                type: "character varying(31)",
                maxLength: 31,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(31)",
                oldMaxLength: 31);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "rank_icon",
                table: "free_company_members",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "rank",
                table: "free_company_members",
                type: "character varying(31)",
                maxLength: 31,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(31)",
                oldMaxLength: 31,
                oldNullable: true);
        }
    }
}
