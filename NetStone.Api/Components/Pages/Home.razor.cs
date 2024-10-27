namespace NetStone.Api.Components.Pages;

/// <summary>
///     Logic for Home page.
/// </summary>
public partial class Home
{
    private const int AnimationDelay = 650;
    private static readonly List<Part> Parts = CreateParts();

    private static readonly List<Reason> Reasons = CreateReasons();

    private static List<Part> CreateParts()
    {
        return
        [
            new Part("Lodestone", "https://eu.finalfantasyxiv.com/lodestone", "Source of all data", "SQUARE ENIX",
                "https://www.square-enix.com"),
            new Part("NetStone", "https://github.com/xivapi/NetStone", "Lodestone Parser", "XIVAPI",
                "https://github.com/xivapi"),
            new Part("NetStone API", null, "API and Caching for NetStone", "Tawmy", "https://tawmy.dev"),
            new Part("NetStone API Client", "https://github.com/Tawmy/NetStone.Api.Client",
                ".NET client for NetStone API", "Tawmy", "https://tawmy.dev")
        ];
    }

    private static List<Reason> CreateReasons()
    {
        return
        [
            new Reason("🙌 Ease of Use",
                "Parsing the Lodestone and handling its data can take up a majority of time spent on a project. This chain of projects built by passionate fans makes retrieving data trivial. Caching is automatic and refreshing the cache a simple query parameter."),
            new Reason("⚡️Performance",
                "Long running operations like retrieving achievements require traversal of many Lodestone pages and can take more than 20 seconds. Retrieving cached data from the API takes less than a tenth of a second."),
            new Reason("📄 Standardised and Documented",
                "All replies, no matter whether cached or not, use the same format. Information about whether data was cached and how long ago data was last refreshed is included. OpenAPI specification and Swagger UI are available."),
            new Reason("❤️ Fair Use",
                "Parsing the Lodestone taxes both your server, but also Square Enix's. NetStone API greatly reduces the amount of times the Lodestone has to be parsed directly, thus also greatly reducing load on Square's servers.")
        ];
    }
}

internal readonly record struct Part(
    string Title,
    string? TitleUrl,
    string Description,
    string Author,
    string AuthorUrl);

internal readonly record struct Reason(string Title, string Description);