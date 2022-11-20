using Godot;
using System.Linq;
using Game;

public partial class ClicheListNode : Control
{
    [Export]
    public PackedScene clicheIndicatorTemplate = null!;

    private City city = null!;
    public City City
    {
        get => city;
        set
        {
            if (city != null)
            {
                city.OnUpdateCliches -= UpdateCityName;
                city.OnUpdateCliches -= UpdateCliches;
            }

            city = value;

            city.OnUpdateCliches += UpdateCityName;
            city.OnUpdateCliches += UpdateCliches;

            UpdateCityName();
            UpdateCliches();
        }
    }

    private RichTextLabel cityName = null!;
    private Node clicheList = null!;

    public ClicheListNode()
    {
        VisibilityChanged += UpdateCityName;
        VisibilityChanged += UpdateCliches;
    }

    public override void _Ready()
    {
        cityName = GetNode<RichTextLabel>("%CityName");
        clicheList = GetNode("%ClicheList");
    }

    private void UpdateCityName()
    {
        if (Visible && city != null)
        {
            cityName.Clear();
            cityName.PushParagraph(HorizontalAlignment.Center, TextDirection.Auto);
            cityName.PushUnderline();
            cityName.PushColor(city.ControlledBy == GameNode.Instance.player ? new Color(0, 1, 0) : new Color(1, 0, 0));
            cityName.AddText(city.name);
            cityName.Pop();
            cityName.Pop();
            cityName.Pop();
        }
    }

    private void UpdateCliches()
    {
        if (Visible && city != null)
        {
            for (int i = clicheList.GetChildCount() - 1; i >= city.clicheStats.Count; i--)
            {
                clicheList.GetChild(i).QueueFree();
            }

            int index = 0;
            foreach (var (cliche, stats) in city.clicheStats.OrderByDescending(x => x.Value.spread))
            {
                ClicheIndicatorNode indicator;
                if (index < clicheList.GetChildCount())
                {
                    indicator = (ClicheIndicatorNode)clicheList.GetChild(index);
                }
                else
                {
                    indicator = (ClicheIndicatorNode)clicheIndicatorTemplate.Instantiate();
                    clicheList.AddChild(indicator);
                }

                indicator.Cliche = cliche;
                indicator.ClicheStats = stats;

                index++;
            }
        }
    }
}
