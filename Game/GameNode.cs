using Godot;
using System.Collections.Generic;

using Timer = System.Timers.Timer;

namespace Game;

public partial class GameNode : Node
{
	public static GameNode Instance { get; private set; } = null!;

	[Export]
	public int totalCities = 40;

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
		//generate cities
		var cities = new List<City>();

		for (int i = 0; i < totalCities; i++)
		{
			var cityName = $"Clichepolis {i + 1}";
			var cityPosition = new Vector2(GD.RandRange(-1500, 1500), GD.RandRange(-1500, 1500));
		
			var tooClose = false;

			do
			{
				tooClose = false;

				foreach (var city in cities)
				{
					if (cityPosition.DistanceTo(city.position) < 400)
					{
						tooClose = true;
						break;
					}
				}

				if (tooClose)
				{
					cityPosition = new Vector2(GD.RandRange(-1500, 1500), GD.RandRange(-1500, 1500));
				}
			}
			while (tooClose);

			cities.Add(new City(cityName, cityPosition));
		}

		foreach (var city in cities)
		{
			//DEBUG code. Remove when done
			city.clicheStats[new Cliche("Cliche A")] = new ClicheCityStats(0.5f, 0.4f);
			city.clicheStats[new Cliche("Cliche B")] = new ClicheCityStats(0.3f, 0.3f);
			city.clicheStats[new Cliche("Cliche C")] = new ClicheCityStats(0.2f, 0.3f);
		}

		cities[0].ConnectTo(cities[1]);

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
