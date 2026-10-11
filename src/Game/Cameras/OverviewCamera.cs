using Godot;

namespace HovercraftRace.Game.Cameras;

/// <summary>Zooms out so a whole <see cref="TileMapLayer"/> fits in the viewport (minimap).</summary>
[GlobalClass]
public partial class OverviewCamera : Camera2D
{
    [Export] public float Padding { get; set; } = 32f;

    private TileMapLayer? _track;

    public override void _Ready() => GetViewport().SizeChanged += Refit;

    public void Frame(TileMapLayer track)
    {
        _track = track;
        Refit();
    }

    private void Refit()
    {
        if (_track?.TileSet is null)
            return;

        var cellSize = (Vector2)_track.TileSet.TileSize;
        var used = _track.GetUsedRect();
        var bounds = new Rect2(_track.GlobalPosition + used.Position * cellSize, used.Size * cellSize).Grow(Padding);
        var viewport = GetViewportRect().Size;

        var zoom = Mathf.Min(viewport.X / bounds.Size.X, viewport.Y / bounds.Size.Y);
        Zoom = new Vector2(zoom, zoom);
        GlobalPosition = bounds.GetCenter();
    }
}
