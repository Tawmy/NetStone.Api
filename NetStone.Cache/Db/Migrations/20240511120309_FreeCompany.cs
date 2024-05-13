using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetStone.Common.Enums;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class FreeCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "top_layer",
                table: "character_free_companies",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "middle_layer",
                table: "character_free_companies",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "bottom_layer",
                table: "character_free_companies",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "free_companies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    lodestone_id = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    slogan = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    tag = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    world = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    crest_top = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    crest_middle = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    crest_bottom = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    formed = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    grand_company = table.Column<GrandCompany>(type: "grand_company", nullable: false),
                    rank = table.Column<short>(type: "smallint", nullable: false),
                    ranking_monthly = table.Column<short>(type: "smallint", nullable: true),
                    ranking_weekly = table.Column<short>(type: "smallint", nullable: true),
                    recruitment = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    active_member_count = table.Column<short>(type: "smallint", nullable: false),
                    active_state = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    estate_name = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    estate_greeting = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    estate_plot = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_free_companies", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_free_companies_lodestone_id",
                table: "free_companies",
                column: "lodestone_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "free_companies");

            migrationBuilder.AlterColumn<string>(
                name: "top_layer",
                table: "character_free_companies",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "middle_layer",
                table: "character_free_companies",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "bottom_layer",
                table: "character_free_companies",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);
        }
    }
}
