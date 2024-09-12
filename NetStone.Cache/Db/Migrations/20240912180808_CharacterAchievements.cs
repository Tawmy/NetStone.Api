using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class CharacterAchievements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "character_achievements_updated_at",
                table: "characters",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "character_achievements",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    character_lodestone_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    character_id = table.Column<int>(type: "integer", nullable: true),
                    achievement_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    name = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: false),
                    database_link = table.Column<string>(type: "text", nullable: false),
                    time_achieved = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_achievements", x => x.id);
                    table.ForeignKey(
                        name: "fk_character_achievements_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_character_achievements_character_id",
                table: "character_achievements",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_achievements_character_lodestone_id_achievement_id",
                table: "character_achievements",
                columns: new[] { "character_lodestone_id", "achievement_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_achievements");

            migrationBuilder.DropColumn(
                name: "character_achievements_updated_at",
                table: "characters");
        }
    }
}
