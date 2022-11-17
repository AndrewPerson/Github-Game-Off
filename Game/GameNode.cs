using Godot;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Collections.Generic;

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
	public int minCityNameSyllables = 3;

	[Export]
	public int maxCityNameSyllables = 5;

	[Export]
	public PackedScene cityTemplate = null!;

	public List<City> cities = new();
	public List<Cliche> cliches = new();

    public Timer timer = new();

    public GameNode()
    {
        Instance = this;

        timer.Interval = 200;
        timer.Elapsed += (_, _) => OnTimerElapsed();
        timer.Start();
    }

    public override void _Ready()
    {
        cities = GenerateCities();
        RenderCities();
    }

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

        if (disposing) timer.Dispose();
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
			var cityNameBuilder = new StringBuilder();
			var syllableCount = GD.RandRange(minCityNameSyllables, maxCityNameSyllables);
			for (int x = 0; x < syllableCount; x++)
			{
				cityNameBuilder.Append(cityNameSyllables[GD.Randi() % cityNameSyllables.Length]);
			}

			var cityName = cityNameBuilder.ToString();

			while (usedNames.Contains(cityName))
			{
				cityName = $"New {cityName}";
			}

			usedNames.Add(cityName);

			cityName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cityName);
			var cityPosition = new Vector2(GD.RandRange(-1500, 1500), GD.RandRange(-1500, 1500));
		
			while (cities.Any(city => city.position.DistanceTo(cityPosition) < 400))
			{
				cityPosition = new Vector2(GD.RandRange(-1500, 1500), GD.RandRange(-1500, 1500));
			}

			//TODO Generate like vectors
			cities.Add(new City(cityName, cityPosition, new()));
		}

		foreach (var city in cities)
		{
			//DEBUG code. Remove when done
			city.clicheStats[new Cliche("Cliche A")] = new ClicheCityStats(0.5f, 0.1f);
			city.clicheStats[new Cliche("Cliche B")] = new ClicheCityStats(0.3f, 0.3f);
			city.clicheStats[new Cliche("Cliche C")] = new ClicheCityStats(0.2f, 0.3f);
		}

		cities[0].ConnectTo(cities[1]);
        cities[1].ConnectTo(cities[0]);

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
}
