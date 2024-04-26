using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterGear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "character_gears",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    character_id = table.Column<int>(type: "integer", nullable: false),
                    slot = table.Column<int>(type: "integer", nullable: false),
                    item_name = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: false),
                    item_database_link = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_hq = table.Column<bool>(type: "boolean", nullable: true),
                    stripped_item_name = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true),
                    glamour_database_link = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    glamour_name = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true),
                    creator_name = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: true),
                    materia1 = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    materia2 = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    materia3 = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    materia4 = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    materia5 = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_gears", x => x.id);
                    table.ForeignKey(
                        name: "fk_character_gears_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_character_gears_character_id",
                table: "character_gears",
                column: "character_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_gears");
        }
    }
}
