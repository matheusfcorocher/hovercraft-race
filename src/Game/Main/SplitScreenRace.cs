using Godot;

namespace HovercraftRace.Game.Main;

/// <summary>
/// Composition root. The world is simulated in an off-screen viewport and every
/// on-screen view renders that same <see cref="World2D"/> through its own camera.
/// </summary>
[GlobalClass]
public partial class SplitScreenRace : Node
{
    [Export] public SubViewport? Simulation { get; set; }

    [Export] public RaceWorld? World { get; set; }

    [Export] public PlayerView? LeftView { get; set; }

    [Export] public PlayerView? RightView { get; set; }

    [Export] public MinimapView? Minimap { get; set; }

    public override void _Ready()
    {
        var world = Simulation!.World2D;
        LeftView!.Follow(world, World!.PlayerTwo!);
        RightView!.Follow(world, World.PlayerOne!);
        Minimap!.Frame(world, World.Track!);
    }
}
