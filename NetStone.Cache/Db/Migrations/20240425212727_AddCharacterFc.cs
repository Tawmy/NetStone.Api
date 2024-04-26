using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterFc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "character_free_companies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    character_id = table.Column<int>(type: "integer", nullable: false),
                    lodestone_id = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    name = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    link = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    top_layer = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    middle_layer = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    bottom_layer = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_free_companies", x => x.id);
                    table.ForeignKey(
                        name: "fk_character_free_companies_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_character_free_companies_character_id",
                table: "character_free_companies",
                column: "character_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_free_companies");
        }
    }
}
