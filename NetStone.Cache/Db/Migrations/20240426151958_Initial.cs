using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetStone.Common.Enums;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:class_job", "gladiator,pugilist,marauder,lancer,archer,conjurer,thaumaturge,carpenter,blacksmith,armorer,goldsmith,leatherworker,weaver,alchemist,culinarian,miner,botanist,fisher,paladin,monk,warrior,dragoon,bard,white_mage,black_mage,arcanist,summoner,scholar,rogue,ninja,machinist,dark_knight,astrologian,samurai,red_mage,blue_mage,gunbreaker,dancer,reaper,sage")
                .Annotation("Npgsql:Enum:grand_company", "no_affiliation,maelstrom,order_of_the_twin_adder,immortal_flames");

            migrationBuilder.CreateTable(
                name: "characters",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    lodestone_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    active_class_job = table.Column<ClassJob>(type: "class_job", nullable: false),
                    active_class_job_level = table.Column<short>(type: "smallint", nullable: false),
                    active_class_job_icon = table.Column<string>(type: "character varying(127)", maxLength: 127, nullable: false),
                    avatar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    bio = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    grand_company = table.Column<GrandCompany>(type: "grand_company", nullable: false),
                    grand_company_rank = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true),
                    guardian_deity_name = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: false),
                    guardian_deity_icon = table.Column<string>(type: "character varying(127)", maxLength: 127, nullable: false),
                    name = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    nameday = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: false),
                    portrait = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    pvp_team = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    race_clan_gender = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    server = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    title = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true),
                    town_name = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    town_icon = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    character_class_jobs_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_characters", x => x.id);
                });

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

            migrationBuilder.CreateTable(
                name: "character_class_jobs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    character_id = table.Column<int>(type: "integer", nullable: false),
                    class_job = table.Column<ClassJob>(type: "class_job", nullable: false),
                    is_job_unlocked = table.Column<bool>(type: "boolean", nullable: false),
                    level = table.Column<short>(type: "smallint", nullable: false),
                    exp_current = table.Column<int>(type: "integer", nullable: false),
                    exp_max = table.Column<int>(type: "integer", nullable: false),
                    exp_to_go = table.Column<int>(type: "integer", nullable: false),
                    is_specialized = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_class_jobs", x => x.id);
                    table.ForeignKey(
                        name: "fk_character_class_jobs_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    top_layer = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    middle_layer = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    bottom_layer = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
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
                name: "ix_character_attributes_character_id",
                table: "character_attributes",
                column: "character_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_character_class_jobs_character_id",
                table: "character_class_jobs",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_free_companies_character_id",
                table: "character_free_companies",
                column: "character_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_character_gears_character_id_slot",
                table: "character_gears",
                columns: new[] { "character_id", "slot" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_characters_lodestone_id",
                table: "characters",
                column: "lodestone_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_attributes");

            migrationBuilder.DropTable(
                name: "character_class_jobs");

            migrationBuilder.DropTable(
                name: "character_free_companies");

            migrationBuilder.DropTable(
                name: "character_gears");

            migrationBuilder.DropTable(
                name: "characters");
        }
    }
}
