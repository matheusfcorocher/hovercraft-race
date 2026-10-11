using Godot;
using HovercraftRace.Game.Cameras;
using HovercraftRace.Game.UI;
using HovercraftRace.Game.Vehicles;

namespace HovercraftRace.Game.Main;

/// <summary>One half of the split screen: a camera on a shared world plus that racer's HUD.</summary>
[GlobalClass]
public partial class PlayerView : Control
{
    [Export] public SubViewport? Viewport { get; set; }

    [Export] public FollowCamera? Camera { get; set; }

    [Export] public LapCounterHud? Hud { get; set; }

    public void Follow(World2D world, Hovercraft racer)
    {
        Viewport!.World2D = world;
        Camera!.Target = racer;
        Hud!.Bind(racer);
    }
}
