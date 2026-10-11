using System.Numerics;

namespace HovercraftRace.Core.Race;

public enum CrossingDirection
{
    Forward,
    Backward,
}

public static class FinishLineCrossing
{
    /// <summary>A craft moving with (or perpendicular to) the race direction crosses forward.</summary>
    public static CrossingDirection Classify(Vector2 velocity, Vector2 raceDirection) =>
        Vector2.Dot(velocity, raceDirection) >= 0f ? CrossingDirection.Forward : CrossingDirection.Backward;
}
