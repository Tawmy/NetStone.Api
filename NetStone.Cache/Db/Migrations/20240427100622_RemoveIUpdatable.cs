using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIUpdatable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "character_class_jobs");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "character_class_jobs");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "characters",
                newName: "character_updated_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "character_updated_at",
                table: "characters",
                newName: "created_at");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "characters",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "character_class_jobs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "character_class_jobs",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
