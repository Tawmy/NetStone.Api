using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClassJobCharacterRequirement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "character_id",
                table: "character_class_jobs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "character_lodestone_id",
                table: "character_class_jobs",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_character_class_jobs_character_lodestone_id_class_job",
                table: "character_class_jobs",
                columns: new[] { "character_lodestone_id", "class_job" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_character_class_jobs_character_lodestone_id_class_job",
                table: "character_class_jobs");

            migrationBuilder.DropColumn(
                name: "character_lodestone_id",
                table: "character_class_jobs");

            migrationBuilder.AlterColumn<int>(
                name: "character_id",
                table: "character_class_jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
