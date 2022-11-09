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

        timer.Interval = 1000;
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
        return new List<City>
        {
            new City("Clichepolis A", new Vector2(0, 0)),
            new City("Clichepolis B", new Vector2(500, 300))
        };
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