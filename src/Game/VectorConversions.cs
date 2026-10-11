namespace HovercraftRace.Game;

/// <summary>Bridges Godot vectors and the engine-independent Core (System.Numerics).</summary>
public static class VectorConversions
{
    public static System.Numerics.Vector2 ToNumerics(this Godot.Vector2 v) => new(v.X, v.Y);

    public static Godot.Vector2 ToGodot(this System.Numerics.Vector2 v) => new(v.X, v.Y);
}
