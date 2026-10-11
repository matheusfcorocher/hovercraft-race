using Godot;
using HovercraftRace.Game.Cameras;

namespace HovercraftRace.Game.Main;

[GlobalClass]
public partial class MinimapView : Control
{
    [Export] public SubViewport? Viewport { get; set; }

    [Export] public OverviewCamera? Camera { get; set; }

    public void Frame(World2D world, TileMapLayer track)
    {
        Viewport!.World2D = world;
        Camera!.Frame(track);
    }
}
