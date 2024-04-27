using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterMinions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "character_minions_updated_at",
                table: "characters",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "character_minions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    character_lodestone_id = table.Column<string>(type: "text", nullable: false),
                    character_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_minions", x => x.id);
                    table.ForeignKey(
                        name: "fk_character_minions_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_character_minions_character_id",
                table: "character_minions",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_minions_character_lodestone_id_name",
                table: "character_minions",
                columns: new[] { "character_lodestone_id", "name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_minions");

            migrationBuilder.DropColumn(
                name: "character_minions_updated_at",
                table: "characters");
        }
    }
}
