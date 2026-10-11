using System.Numerics;

namespace HovercraftRace.Core.Physics;

/// <param name="Velocity">Pixels per second, world space.</param>
/// <param name="Heading">Unit vector the craft is facing.</param>
public readonly record struct MotionState(Vector2 Velocity, Vector2 Heading)
{
    public float Speed => Velocity.Length();

    /// <summary>Heading as an angle in radians, matching Godot's <c>Node2D.Rotation</c>.</summary>
    public float Rotation => MathF.Atan2(Heading.Y, Heading.X);

    public static MotionState AtRest(float rotation) =>
        new(Vector2.Zero, new Vector2(MathF.Cos(rotation), MathF.Sin(rotation)));
}
