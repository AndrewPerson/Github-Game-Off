using Godot;

namespace Game;

public partial class IndicatorSliceNode : ColorRect
{
    private Color colour;
    public Color Colour
    {
        get => colour;
        set
        {
            colour = value;
            UpdateColour();
        }
    }

    private float percentage = 0;
    public float Percentage
    {
        get => percentage;
        set
        {
            percentage = value;
            UpdatePercentage();
        }
    }

    private float percentageOffset = 0;
    public float PercentageOffset
    {
        get => percentageOffset;
        set
        {
            percentageOffset = value;
            UpdatePercentageOffset();
        }
    }

    private ShaderMaterial material = null!;

    public override void _Ready()
    {
        material = (ShaderMaterial)Material.Duplicate();
        Material = material;

        UpdateColour();
        UpdatePercentage();
        UpdatePercentageOffset();
    }

    private void UpdateColour() => Color = colour;

    private void UpdatePercentage() => material.SetShaderParameter("percentage", percentage);

    private void UpdatePercentageOffset() => material.SetShaderParameter("percentage_offset", percentageOffset);
}
