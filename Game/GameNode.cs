using Godot;
using System.Collections.Generic;

using Timer = System.Timers.Timer;

namespace Game;

public partial class GameNode : Node
{
    public static GameNode Instance { get; private set; } = null!;

    [Export]
    public PackedScene cityTemplate = null!;

    public List<City> cities = new();
    public List<Cliche> cliches = new();

    public Timer timer = new();

    public override void _Ready()
    {
        Instance = this;

        timer.Interval = 200;
        timer.Elapsed += (sender, e) => OnTimerElapsed();
        timer.Start();

        cities = GenerateCities();
        RenderCities();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        timer.Dispose();
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
        var cities = new List<City>
        {
            new City("Clichepolis A", new Vector2(100, 100)),
            new City("Clichepolis B", new Vector2(600, 600))
        };

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