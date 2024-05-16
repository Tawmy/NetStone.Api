using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class FcRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "full_free_company_id",
                table: "characters",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_characters_full_free_company_id",
                table: "characters",
                column: "full_free_company_id");

            migrationBuilder.AddForeignKey(
                name: "fk_characters_free_companies_full_free_company_id",
                table: "characters",
                column: "full_free_company_id",
                principalTable: "free_companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_characters_free_companies_full_free_company_id",
                table: "characters");

            migrationBuilder.DropIndex(
                name: "ix_characters_full_free_company_id",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "full_free_company_id",
                table: "characters");
        }
    }
}
