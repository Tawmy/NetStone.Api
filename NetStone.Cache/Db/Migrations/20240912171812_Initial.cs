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
                .Annotation("Npgsql:Enum:class_job", "gladiator,pugilist,marauder,lancer,archer,conjurer,thaumaturge,carpenter,blacksmith,armorer,goldsmith,leatherworker,weaver,alchemist,culinarian,miner,botanist,fisher,paladin,monk,warrior,dragoon,bard,white_mage,black_mage,arcanist,summoner,scholar,rogue,ninja,machinist,dark_knight,astrologian,samurai,red_mage,blue_mage,gunbreaker,dancer,reaper,sage,viper,pictomancer")
                .Annotation("Npgsql:Enum:gear_slot", "main_hand,off_hand,head,body,hands,legs,feet,earrings,necklace,bracelets,ring1,ring2,soul_crystal")
                .Annotation("Npgsql:Enum:gender", "male,female")
                .Annotation("Npgsql:Enum:grand_company", "no_affiliation,maelstrom,order_of_the_twin_adder,immortal_flames")
                .Annotation("Npgsql:Enum:race", "hyur,elezen,lalafell,miqote,roegadyn,au_ra,hrothgar,viera")
                .Annotation("Npgsql:Enum:tribe", "midlander,highlander,wildwood,duskwight,plainsfolk,dunesfolk,seeker_of_the_sun,keeper_of_the_moon,sea_wolf,hellsguard,raen,xaela,helions,the_lost,rava,veena");

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
                    crest_top = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    crest_middle = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    crest_bottom = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
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
                    estate_plot = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true),
                    free_company_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    free_company_members_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    focus = table.Column<int>(type: "integer", nullable: false),
                    maelstrom_progress = table.Column<short>(type: "smallint", nullable: false),
                    maelstrom_rank = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    twin_adder_progress = table.Column<short>(type: "smallint", nullable: false),
                    twin_adder_rank = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    immortal_flames_progress = table.Column<short>(type: "smallint", nullable: false),
                    immortal_flames_rank = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_free_companies", x => x.id);
                });

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
                    full_free_company_id = table.Column<int>(type: "integer", nullable: true),
                    grand_company = table.Column<GrandCompany>(type: "grand_company", nullable: false),
                    grand_company_rank = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true),
                    guardian_deity_name = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: false),
                    guardian_deity_icon = table.Column<string>(type: "character varying(127)", maxLength: 127, nullable: false),
                    name = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    nameday = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: false),
                    portrait = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    pvp_team = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    race = table.Column<Race>(type: "race", nullable: false),
                    tribe = table.Column<Tribe>(type: "tribe", nullable: false),
                    gender = table.Column<Gender>(type: "gender", nullable: false),
                    server = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    title = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true),
                    town_name = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    town_icon = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    character_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    character_class_jobs_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    character_minions_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    character_mounts_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_characters", x => x.id);
                    table.ForeignKey(
                        name: "fk_characters_free_companies_full_free_company_id",
                        column: x => x.full_free_company_id,
                        principalTable: "free_companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    character_lodestone_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    character_id = table.Column<int>(type: "integer", nullable: true),
                    class_job = table.Column<ClassJob>(type: "class_job", nullable: false),
                    is_job_unlocked = table.Column<bool>(type: "boolean", nullable: false),
                    level = table.Column<short>(type: "smallint", nullable: false),
                    exp_current = table.Column<int>(type: "integer", nullable: false),
                    exp_max = table.Column<int>(type: "integer", nullable: false),
                    exp_to_go = table.Column<int>(type: "integer", nullable: false),
                    is_specialized = table.Column<bool>(type: "boolean", nullable: false)
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
                    slot = table.Column<GearSlot>(type: "gear_slot", nullable: false),
                    item_name = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: false),
                    item_database_link = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_hq = table.Column<bool>(type: "boolean", nullable: true),
                    stripped_item_name = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true),
                    glamour_database_link = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    glamour_name = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true),
                    creator_name = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: true),
                    materia1 = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true),
                    materia2 = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true),
                    materia3 = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true),
                    materia4 = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true),
                    materia5 = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "character_minions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    character_lodestone_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "character_mounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    character_lodestone_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
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
                    rank = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    rank_icon = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
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
                name: "ix_character_attributes_character_id",
                table: "character_attributes",
                column: "character_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_character_class_jobs_character_id",
                table: "character_class_jobs",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_class_jobs_character_lodestone_id_class_job",
                table: "character_class_jobs",
                columns: new[] { "character_lodestone_id", "class_job" },
                unique: true);

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
                name: "ix_character_minions_character_id",
                table: "character_minions",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_minions_character_lodestone_id_name",
                table: "character_minions",
                columns: new[] { "character_lodestone_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_character_mounts_character_id",
                table: "character_mounts",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_mounts_character_lodestone_id_name",
                table: "character_mounts",
                columns: new[] { "character_lodestone_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_characters_full_free_company_id",
                table: "characters",
                column: "full_free_company_id");

            migrationBuilder.CreateIndex(
                name: "ix_characters_lodestone_id",
                table: "characters",
                column: "lodestone_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_free_companies_lodestone_id",
                table: "free_companies",
                column: "lodestone_id",
                unique: true);

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
                name: "character_attributes");

            migrationBuilder.DropTable(
                name: "character_class_jobs");

            migrationBuilder.DropTable(
                name: "character_free_companies");

            migrationBuilder.DropTable(
                name: "character_gears");

            migrationBuilder.DropTable(
                name: "character_minions");

            migrationBuilder.DropTable(
                name: "character_mounts");

            migrationBuilder.DropTable(
                name: "free_company_members");

            migrationBuilder.DropTable(
                name: "characters");

            migrationBuilder.DropTable(
                name: "free_companies");
        }
    }
}
