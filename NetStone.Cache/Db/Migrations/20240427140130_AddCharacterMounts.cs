using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterMounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "character_mounts_updated_at",
                table: "characters",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "character_mounts",
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
                    table.PrimaryKey("pk_character_mounts", x => x.id);
                    table.ForeignKey(
                        name: "fk_character_mounts_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_character_mounts_character_id",
                table: "character_mounts",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_mounts_character_lodestone_id_name",
                table: "character_mounts",
                columns: new[] { "character_lodestone_id", "name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_mounts");

            migrationBuilder.DropColumn(
                name: "character_mounts_updated_at",
                table: "characters");
        }
    }
}
