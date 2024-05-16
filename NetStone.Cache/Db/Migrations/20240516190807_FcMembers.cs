using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class FcMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "free_company_members_updated_at",
                table: "free_companies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "free_company_members",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    character_lodestone_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    full_character_id = table.Column<int>(type: "integer", nullable: true),
                    free_company_lodestone_id = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    free_company_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    rank = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    rank_icon = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    server = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    data_center = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    avatar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_free_company_members", x => x.id);
                    table.ForeignKey(
                        name: "fk_free_company_members_characters_full_character_id",
                        column: x => x.full_character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_free_company_members_free_companies_free_company_id",
                        column: x => x.free_company_id,
                        principalTable: "free_companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_free_company_members_character_lodestone_id_free_company_lo",
                table: "free_company_members",
                columns: new[] { "character_lodestone_id", "free_company_lodestone_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_free_company_members_free_company_id",
                table: "free_company_members",
                column: "free_company_id");

            migrationBuilder.CreateIndex(
                name: "ix_free_company_members_full_character_id",
                table: "free_company_members",
                column: "full_character_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "free_company_members");

            migrationBuilder.DropColumn(
                name: "free_company_members_updated_at",
                table: "free_companies");
        }
    }
}
