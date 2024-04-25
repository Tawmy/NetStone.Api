using Microsoft.EntityFrameworkCore.Migrations;
using NetStone.StaticData;
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
                .Annotation("Npgsql:Enum:class_job", "none,gladiator,pugilist,marauder,lancer,archer,conjurer,thaumaturge,carpenter,blacksmith,armorer,goldsmith,leatherworker,weaver,alchemist,culinarian,miner,botanist,fisher,paladin,monk,warrior,dragoon,bard,white_mage,black_mage,arcanist,summoner,scholar,rogue,ninja,machinist,dark_knight,astrologian,samurai,red_mage,blue_mage,gunbreaker,dancer,reaper,sage")
                .Annotation("Npgsql:Enum:grand_company", "none,no_affiliation,maelstrom,order_of_the_twin_adder,immortal_flames");

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
                    name = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false),
                    nameday = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: false),
                    portrait = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    pvp_team = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    race_clan_gender = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    server = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    title = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: true),
                    town_name = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    town_icon = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_characters", x => x.id);
                });

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
                name: "characters");
        }
    }
}
