using Godot;
using HovercraftRace.Game.Vehicles;

namespace HovercraftRace.Game.UI;

/// <summary>Shows one racer's completed laps, updated on <see cref="Hovercraft.LapCompleted"/>.</summary>
[GlobalClass]
public partial class LapCounterHud : Control
{
    [Export] public Label? LapsLabel { get; set; }

    private Hovercraft? _racer;

    public void Bind(Hovercraft racer)
    {
        if (_racer is not null)
            _racer.LapCompleted -= ShowLaps;

        _racer = racer;
        _racer.LapCompleted += ShowLaps;
        ShowLaps(racer.Laps.CompletedLaps);
    }

    public override void _ExitTree()
    {
        if (_racer is not null)
            _racer.LapCompleted -= ShowLaps;
    }

    private void ShowLaps(int laps)
    {
        if (LapsLabel is not null)
            LapsLabel.Text = laps.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}
