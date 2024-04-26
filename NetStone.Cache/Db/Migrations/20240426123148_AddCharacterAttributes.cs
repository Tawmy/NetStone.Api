using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "character_attributes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    character_id = table.Column<int>(type: "integer", nullable: false),
                    strength = table.Column<int>(type: "integer", nullable: false),
                    dexterity = table.Column<int>(type: "integer", nullable: false),
                    vitality = table.Column<int>(type: "integer", nullable: false),
                    intelligence = table.Column<int>(type: "integer", nullable: false),
                    mind = table.Column<int>(type: "integer", nullable: false),
                    critical_hit_rate = table.Column<int>(type: "integer", nullable: false),
                    determination = table.Column<int>(type: "integer", nullable: false),
                    direct_hit_rate = table.Column<int>(type: "integer", nullable: false),
                    defense = table.Column<int>(type: "integer", nullable: false),
                    magic_defense = table.Column<int>(type: "integer", nullable: false),
                    attack_power = table.Column<int>(type: "integer", nullable: false),
                    skill_speed = table.Column<int>(type: "integer", nullable: false),
                    attack_magic_potency = table.Column<int>(type: "integer", nullable: false),
                    healing_magic_potency = table.Column<int>(type: "integer", nullable: false),
                    spell_speed = table.Column<int>(type: "integer", nullable: true),
                    tenacity = table.Column<int>(type: "integer", nullable: true),
                    piety = table.Column<int>(type: "integer", nullable: true),
                    hp = table.Column<int>(type: "integer", nullable: false),
                    mp_gp_cp = table.Column<int>(type: "integer", nullable: false),
                    mp_gp_cp_parameter_name = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_attributes", x => x.id);
                    table.ForeignKey(
                        name: "fk_character_attributes_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_character_attributes_character_id",
                table: "character_attributes",
                column: "character_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_attributes");
        }
    }
}
