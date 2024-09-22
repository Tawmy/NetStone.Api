namespace NetStone.Test.DataGenerators;

public class FreeCompanyTestsDataGenerator : TheoryData<string>
{
    public FreeCompanyTestsDataGenerator()
    {
        Add("9231253336202818312"); // Dust Bunnies, Phoenix
        Add("9228438586435670171"); // Anxiety's End, Lich
        Add("9232660711086325985"); // A Darling A Day, Odin
        Add("9228438586435573203"); // Blue Moon, Lich

        // the following free companies have been chosen randomly on the Lodestone, since I don't know people overseas
        Add("9234490298434849357"); // The Tabard Cloud, Cactuar
        Add("9226327524110238568"); // Grug Knights, Ravana
        Add("9234912510900035212"); // Zinchan Family, Mandragora
        Add("9231253336202818296"); // Friends Unleashed, Phoenix <- testing Lodestone recruitment
    }
}