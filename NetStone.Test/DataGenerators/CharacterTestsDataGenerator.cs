namespace NetStone.Test.DataGenerators;

internal sealed class CharacterTestsDataGenerator : TheoryData<string>
{
    public CharacterTestsDataGenerator()
    {
        Add("28812634"); // Alyx Bergen, Phoenix
        Add("28915387"); // Halvar Ragnar, Phoenix
        Add("44675801"); // Sigyn Leigheas, Phoenix
        Add("45386124"); // Testerinus Maximus, Twintania
        Add("51569642"); // Max Bergen, Phoenix
        Add("42728664"); // Alyx Bergen, Aether
        Add("28835226"); // Hena Wilbert, Phoenix
        Add("19060231"); // Lotharius Fordragon, Lich
        Add("44561456"); // Kalamari Iratus, Phoenix
        Add("44756827"); // Silver Arkrome, Phantom
        Add("2648055"); // E'rio Ninfix, Phoenix
        Add("18188832"); // Rhayn Akiba, Lich

        // the following characters have been chosen randomly on the Lodestone, since I don't know people overseas
        Add("3386216"); // Ace Ataeru, Cactuar
        Add("24196435"); // Beckett Medani, Ravana
        Add("31894233"); // Zeroe Tukassa, Mandragora
    }
}