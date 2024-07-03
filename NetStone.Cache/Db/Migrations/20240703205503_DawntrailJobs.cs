using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    /// <inheritdoc />
    public partial class DawntrailJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:class_job", "gladiator,pugilist,marauder,lancer,archer,conjurer,thaumaturge,carpenter,blacksmith,armorer,goldsmith,leatherworker,weaver,alchemist,culinarian,miner,botanist,fisher,paladin,monk,warrior,dragoon,bard,white_mage,black_mage,arcanist,summoner,scholar,rogue,ninja,machinist,dark_knight,astrologian,samurai,red_mage,blue_mage,gunbreaker,dancer,reaper,sage,viper,pictomancer")
                .Annotation("Npgsql:Enum:grand_company", "no_affiliation,maelstrom,order_of_the_twin_adder,immortal_flames")
                .OldAnnotation("Npgsql:Enum:class_job", "gladiator,pugilist,marauder,lancer,archer,conjurer,thaumaturge,carpenter,blacksmith,armorer,goldsmith,leatherworker,weaver,alchemist,culinarian,miner,botanist,fisher,paladin,monk,warrior,dragoon,bard,white_mage,black_mage,arcanist,summoner,scholar,rogue,ninja,machinist,dark_knight,astrologian,samurai,red_mage,blue_mage,gunbreaker,dancer,reaper,sage")
                .OldAnnotation("Npgsql:Enum:grand_company", "no_affiliation,maelstrom,order_of_the_twin_adder,immortal_flames");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:class_job", "gladiator,pugilist,marauder,lancer,archer,conjurer,thaumaturge,carpenter,blacksmith,armorer,goldsmith,leatherworker,weaver,alchemist,culinarian,miner,botanist,fisher,paladin,monk,warrior,dragoon,bard,white_mage,black_mage,arcanist,summoner,scholar,rogue,ninja,machinist,dark_knight,astrologian,samurai,red_mage,blue_mage,gunbreaker,dancer,reaper,sage")
                .Annotation("Npgsql:Enum:grand_company", "no_affiliation,maelstrom,order_of_the_twin_adder,immortal_flames")
                .OldAnnotation("Npgsql:Enum:class_job", "gladiator,pugilist,marauder,lancer,archer,conjurer,thaumaturge,carpenter,blacksmith,armorer,goldsmith,leatherworker,weaver,alchemist,culinarian,miner,botanist,fisher,paladin,monk,warrior,dragoon,bard,white_mage,black_mage,arcanist,summoner,scholar,rogue,ninja,machinist,dark_knight,astrologian,samurai,red_mage,blue_mage,gunbreaker,dancer,reaper,sage,viper,pictomancer")
                .OldAnnotation("Npgsql:Enum:grand_company", "no_affiliation,maelstrom,order_of_the_twin_adder,immortal_flames");
        }
    }
}
