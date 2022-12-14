using Godot;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using Proxem.NumNet;
using Proxem.Word2Vec;

using Timer = System.Timers.Timer;

namespace Game;

public partial class GameNode : Node
{
    public readonly string[] cityNameSyllables = new string[]
    {
        "ca", "do", "ica", "ip",
        "lo", "lus", "ma", "mo",
        "mus", "nu", "pi", "re",
        "res", "ro", "sum", "te"
    };

    public static GameNode Instance { get; private set; } = null!;

    [Export]
    public int totalCities = 40;

    [Export]
    public int numCityLikeVectorsMax = 7;

    [Export]
    public int numCityLikeVectorsMin = 2;

    [Export]
    public int mapWidth= 3000;
    [Export]
    public int mapHeight = 3000;

    [Export]
    public int minCityNameSyllables = 3;

    [Export]
    public int maxCityNameSyllables = 5;

    [Export]
    public PackedScene cityTemplate = null!;

    [Export]
    public ConnectionCreatorNode connectionCreator = null!;

    public Word2Vec model;

    //TODO Make this based off player input.
    public Player player = new("Andrew");
    public List<City> cities = new();
    public List<Cliche> cliches = new();

    public Timer timer = new();

    public GameNode()
    {
        Instance = this;

        Catalyst.Models.English.Register();

        model = Word2Vec.LoadBinary("NLP/model.bin", normalize: true, encoding: Encoding.UTF8);

        timer.Interval = 200;
        timer.Elapsed += (_, _) => OnTimerElapsed();
        timer.Start();
    }

    public override void _Ready()
    {
        cities = GenerateCities();
        RenderCities();
    }

    private void OnTimerElapsed()
    {
        foreach (var city in cities)
        {
            city.CalculateInternalClicheSpreads();
        }
    }

    private List<City> GenerateCities()
    {
        var usedNames = new HashSet<string>();
        var cities = new List<City>();
        for (int i = 0; i < totalCities; i++)
        {
            // Generate a random name for the city
            string cityName;
            do
            {
                var cityNameBuilder = new StringBuilder();
                var syllableCount = GD.RandRange(minCityNameSyllables, maxCityNameSyllables);
                for (int x = 0; x < syllableCount; x++)
                {
                    cityNameBuilder.Append(cityNameSyllables[GD.Randi() % cityNameSyllables.Length]);
                }

                cityName = cityNameBuilder.ToString();
            }
            while (usedNames.Contains(cityName));

            usedNames.Add(cityName);

            cityName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cityName);

            // Generate a random position for the city
            var cityPosition = new Vector2(GD.RandRange(mapHeight / 2 , mapHeight / -2), GD.RandRange(mapWidth / 2 , mapWidth / -2));
            while (cities.Any(city => city.position.DistanceTo(cityPosition) < 400))
            {
                cityPosition = new Vector2(GD.RandRange(mapHeight / 2 , mapHeight / -2), GD.RandRange(mapWidth / 2 , mapWidth / -2));
            }

            var likeWords = new List<string>();
            for (int x = 0; x < GD.RandRange(numCityLikeVectorsMin, numCityLikeVectorsMax); x++)
            {
                var word = model.Text[GD.RandRange(0, model.Text.Length - 1)];
                likeWords.Add(word);
            }

            //Add City to list
            cities.Add(new City(cityName, cityPosition, likeWords));
        }

        foreach (var city in cities)
        {
            var cliche = new Cliche("A watched pot never boils", player);
            city.AddCliche(cliche, cliche.GetCatchiness(city));

            var cliche2 = new Cliche("You can't teach an old dog new tricks", new Player("Opponent"));
            city.AddCliche(cliche2, cliche2.GetCatchiness(city));

            //DEBUG code. Remove when done
            // city.clicheStats[new Cliche("Cliche A", GD.Randf() < 0.5 ? player : new Player("Opponent"))] = new ClicheCityStats(0.5f, 0.01f);
            // city.clicheStats[new Cliche("Cliche B", GD.Randf() < 0.5 ? player : new Player("Opponent"))] = new ClicheCityStats(0.3f, 0.3f);
            // city.clicheStats[new Cliche("Cliche C", GD.Randf() < 0.5 ? player : new Player("Opponent"))] = new ClicheCityStats(0.2f, 0.3f);
        }

        return cities;
    }

    private void RenderCities()
    {
        foreach (var city in cities)
        {
            var node = cityTemplate.Instantiate<CityNode>();
            node.city = city;

            AddChild(node);
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing) timer.Dispose();
    }
}
