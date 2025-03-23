using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class v300 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:class_job", "alchemist,arcanist,archer,armorer,astrologian,bard,black_mage,blacksmith,blue_mage,botanist,carpenter,conjurer,culinarian,dancer,dark_knight,dragoon,fisher,gladiator,goldsmith,gunbreaker,lancer,leatherworker,machinist,marauder,miner,monk,ninja,paladin,pictomancer,pugilist,reaper,red_mage,rogue,sage,samurai,scholar,summoner,thaumaturge,viper,warrior,weaver,white_mage")
                .Annotation("Npgsql:Enum:gear_slot", "body,bracelets,earrings,feet,hands,head,legs,main_hand,necklace,off_hand,ring1,ring2,soul_crystal")
                .Annotation("Npgsql:Enum:gender", "female,male")
                .Annotation("Npgsql:Enum:grand_company", "immortal_flames,maelstrom,no_affiliation,order_of_the_twin_adder")
                .Annotation("Npgsql:Enum:race", "au_ra,elezen,hrothgar,hyur,lalafell,miqote,roegadyn,viera")
                .Annotation("Npgsql:Enum:tribe", "dunesfolk,duskwight,helions,hellsguard,highlander,keeper_of_the_moon,midlander,plainsfolk,raen,rava,sea_wolf,seeker_of_the_sun,the_lost,veena,wildwood,xaela")
                .OldAnnotation("Npgsql:Enum:class_job", "gladiator,pugilist,marauder,lancer,archer,conjurer,thaumaturge,carpenter,blacksmith,armorer,goldsmith,leatherworker,weaver,alchemist,culinarian,miner,botanist,fisher,paladin,monk,warrior,dragoon,bard,white_mage,black_mage,arcanist,summoner,scholar,rogue,ninja,machinist,dark_knight,astrologian,samurai,red_mage,blue_mage,gunbreaker,dancer,reaper,sage,viper,pictomancer")
                .OldAnnotation("Npgsql:Enum:gear_slot", "main_hand,off_hand,head,body,hands,legs,feet,earrings,necklace,bracelets,ring1,ring2,soul_crystal")
                .OldAnnotation("Npgsql:Enum:gender", "male,female")
                .OldAnnotation("Npgsql:Enum:grand_company", "no_affiliation,maelstrom,order_of_the_twin_adder,immortal_flames")
                .OldAnnotation("Npgsql:Enum:race", "hyur,elezen,lalafell,miqote,roegadyn,au_ra,hrothgar,viera")
                .OldAnnotation("Npgsql:Enum:tribe", "midlander,highlander,wildwood,duskwight,plainsfolk,dunesfolk,seeker_of_the_sun,keeper_of_the_moon,sea_wolf,hellsguard,raen,xaela,helions,the_lost,rava,veena");

            migrationBuilder.AlterColumn<long>(
                name: "exp_to_go",
                table: "character_class_jobs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "exp_max",
                table: "character_class_jobs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "exp_current",
                table: "character_class_jobs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:class_job", "gladiator,pugilist,marauder,lancer,archer,conjurer,thaumaturge,carpenter,blacksmith,armorer,goldsmith,leatherworker,weaver,alchemist,culinarian,miner,botanist,fisher,paladin,monk,warrior,dragoon,bard,white_mage,black_mage,arcanist,summoner,scholar,rogue,ninja,machinist,dark_knight,astrologian,samurai,red_mage,blue_mage,gunbreaker,dancer,reaper,sage,viper,pictomancer")
                .Annotation("Npgsql:Enum:gear_slot", "main_hand,off_hand,head,body,hands,legs,feet,earrings,necklace,bracelets,ring1,ring2,soul_crystal")
                .Annotation("Npgsql:Enum:gender", "male,female")
                .Annotation("Npgsql:Enum:grand_company", "no_affiliation,maelstrom,order_of_the_twin_adder,immortal_flames")
                .Annotation("Npgsql:Enum:race", "hyur,elezen,lalafell,miqote,roegadyn,au_ra,hrothgar,viera")
                .Annotation("Npgsql:Enum:tribe", "midlander,highlander,wildwood,duskwight,plainsfolk,dunesfolk,seeker_of_the_sun,keeper_of_the_moon,sea_wolf,hellsguard,raen,xaela,helions,the_lost,rava,veena")
                .OldAnnotation("Npgsql:Enum:class_job", "alchemist,arcanist,archer,armorer,astrologian,bard,black_mage,blacksmith,blue_mage,botanist,carpenter,conjurer,culinarian,dancer,dark_knight,dragoon,fisher,gladiator,goldsmith,gunbreaker,lancer,leatherworker,machinist,marauder,miner,monk,ninja,paladin,pictomancer,pugilist,reaper,red_mage,rogue,sage,samurai,scholar,summoner,thaumaturge,viper,warrior,weaver,white_mage")
                .OldAnnotation("Npgsql:Enum:gear_slot", "body,bracelets,earrings,feet,hands,head,legs,main_hand,necklace,off_hand,ring1,ring2,soul_crystal")
                .OldAnnotation("Npgsql:Enum:gender", "female,male")
                .OldAnnotation("Npgsql:Enum:grand_company", "immortal_flames,maelstrom,no_affiliation,order_of_the_twin_adder")
                .OldAnnotation("Npgsql:Enum:race", "au_ra,elezen,hrothgar,hyur,lalafell,miqote,roegadyn,viera")
                .OldAnnotation("Npgsql:Enum:tribe", "dunesfolk,duskwight,helions,hellsguard,highlander,keeper_of_the_moon,midlander,plainsfolk,raen,rava,sea_wolf,seeker_of_the_sun,the_lost,veena,wildwood,xaela");

            migrationBuilder.AlterColumn<int>(
                name: "exp_to_go",
                table: "character_class_jobs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "exp_max",
                table: "character_class_jobs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "exp_current",
                table: "character_class_jobs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
