using Godot;
using System.Collections.Generic;

using Timer = System.Timers.Timer;

namespace Game;

public partial class Game : Node
{
    public static Game Instance { get; private set; } = null!;

    public List<City> cities = new();
    public List<Cliche> cliches = new();

    public Timer timer = new();

    public override void _Ready()
    {
        Instance = this;

        timer.Interval = 1000;
        timer.Elapsed += (sender, e) => OnTimerElapsed();
        timer.Start();

        GenerateCities();
        RenderCities();
    }

    private void OnTimerElapsed()
    {
        foreach (var city in cities)
        {
            city.CalculateInternalClicheSpreads();
        }
    }

    private void GenerateCities()
    {
        throw new System.NotImplementedException("City generation not implemented");
    }

    private void RenderCities()
    {
        throw new System.NotImplementedException("City rendering not implemented");
    }
}  