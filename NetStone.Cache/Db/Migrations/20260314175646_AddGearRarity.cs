using Microsoft.EntityFrameworkCore.Migrations;
using NetStone.Common.Enums;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddGearRarity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:class_job", "alchemist,arcanist,archer,armorer,astrologian,bard,black_mage,blacksmith,blue_mage,botanist,carpenter,conjurer,culinarian,dancer,dark_knight,dragoon,fisher,gladiator,goldsmith,gunbreaker,lancer,leatherworker,machinist,marauder,miner,monk,ninja,paladin,pictomancer,pugilist,reaper,red_mage,rogue,sage,samurai,scholar,summoner,thaumaturge,viper,warrior,weaver,white_mage")
                .Annotation("Npgsql:Enum:gear_rarity", "common,epic,magic,rare,uncommon")
                .Annotation("Npgsql:Enum:gear_slot", "body,bracelets,earrings,facewear,feet,hands,head,legs,main_hand,necklace,off_hand,ring1,ring2,soul_crystal")
                .Annotation("Npgsql:Enum:gender", "female,male")
                .Annotation("Npgsql:Enum:grand_company", "immortal_flames,maelstrom,no_affiliation,order_of_the_twin_adder")
                .Annotation("Npgsql:Enum:race", "au_ra,elezen,hrothgar,hyur,lalafell,miqote,roegadyn,viera")
                .Annotation("Npgsql:Enum:tribe", "dunesfolk,duskwight,helions,hellsguard,highlander,keeper_of_the_moon,midlander,plainsfolk,raen,rava,sea_wolf,seeker_of_the_sun,the_lost,veena,wildwood,xaela")
                .OldAnnotation("Npgsql:Enum:class_job", "alchemist,arcanist,archer,armorer,astrologian,bard,black_mage,blacksmith,blue_mage,botanist,carpenter,conjurer,culinarian,dancer,dark_knight,dragoon,fisher,gladiator,goldsmith,gunbreaker,lancer,leatherworker,machinist,marauder,miner,monk,ninja,paladin,pictomancer,pugilist,reaper,red_mage,rogue,sage,samurai,scholar,summoner,thaumaturge,viper,warrior,weaver,white_mage")
                .OldAnnotation("Npgsql:Enum:gear_slot", "body,bracelets,earrings,facewear,feet,hands,head,legs,main_hand,necklace,off_hand,ring1,ring2,soul_crystal")
                .OldAnnotation("Npgsql:Enum:gender", "female,male")
                .OldAnnotation("Npgsql:Enum:grand_company", "immortal_flames,maelstrom,no_affiliation,order_of_the_twin_adder")
                .OldAnnotation("Npgsql:Enum:race", "au_ra,elezen,hrothgar,hyur,lalafell,miqote,roegadyn,viera")
                .OldAnnotation("Npgsql:Enum:tribe", "dunesfolk,duskwight,helions,hellsguard,highlander,keeper_of_the_moon,midlander,plainsfolk,raen,rava,sea_wolf,seeker_of_the_sun,the_lost,veena,wildwood,xaela");

            migrationBuilder.AddColumn<GearRarity>(
                name: "rarity",
                table: "character_gears",
                type: "gear_rarity",
                nullable: false,
                defaultValue: GearRarity.Common);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rarity",
                table: "character_gears");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:class_job", "alchemist,arcanist,archer,armorer,astrologian,bard,black_mage,blacksmith,blue_mage,botanist,carpenter,conjurer,culinarian,dancer,dark_knight,dragoon,fisher,gladiator,goldsmith,gunbreaker,lancer,leatherworker,machinist,marauder,miner,monk,ninja,paladin,pictomancer,pugilist,reaper,red_mage,rogue,sage,samurai,scholar,summoner,thaumaturge,viper,warrior,weaver,white_mage")
                .Annotation("Npgsql:Enum:gear_slot", "body,bracelets,earrings,facewear,feet,hands,head,legs,main_hand,necklace,off_hand,ring1,ring2,soul_crystal")
                .Annotation("Npgsql:Enum:gender", "female,male")
                .Annotation("Npgsql:Enum:grand_company", "immortal_flames,maelstrom,no_affiliation,order_of_the_twin_adder")
                .Annotation("Npgsql:Enum:race", "au_ra,elezen,hrothgar,hyur,lalafell,miqote,roegadyn,viera")
                .Annotation("Npgsql:Enum:tribe", "dunesfolk,duskwight,helions,hellsguard,highlander,keeper_of_the_moon,midlander,plainsfolk,raen,rava,sea_wolf,seeker_of_the_sun,the_lost,veena,wildwood,xaela")
                .OldAnnotation("Npgsql:Enum:class_job", "alchemist,arcanist,archer,armorer,astrologian,bard,black_mage,blacksmith,blue_mage,botanist,carpenter,conjurer,culinarian,dancer,dark_knight,dragoon,fisher,gladiator,goldsmith,gunbreaker,lancer,leatherworker,machinist,marauder,miner,monk,ninja,paladin,pictomancer,pugilist,reaper,red_mage,rogue,sage,samurai,scholar,summoner,thaumaturge,viper,warrior,weaver,white_mage")
                .OldAnnotation("Npgsql:Enum:gear_rarity", "common,epic,magic,rare,uncommon")
                .OldAnnotation("Npgsql:Enum:gear_slot", "body,bracelets,earrings,facewear,feet,hands,head,legs,main_hand,necklace,off_hand,ring1,ring2,soul_crystal")
                .OldAnnotation("Npgsql:Enum:gender", "female,male")
                .OldAnnotation("Npgsql:Enum:grand_company", "immortal_flames,maelstrom,no_affiliation,order_of_the_twin_adder")
                .OldAnnotation("Npgsql:Enum:race", "au_ra,elezen,hrothgar,hyur,lalafell,miqote,roegadyn,viera")
                .OldAnnotation("Npgsql:Enum:tribe", "dunesfolk,duskwight,helions,hellsguard,highlander,keeper_of_the_moon,midlander,plainsfolk,raen,rava,sea_wolf,seeker_of_the_sun,the_lost,veena,wildwood,xaela");
        }
    }
}
