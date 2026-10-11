using System.Numerics;
using HovercraftRace.Core.Race;

namespace HovercraftRace.Core.Tests.Race;

public class FinishLineCrossingTests
{
    private static readonly Vector2 RaceDirection = new(0f, -1f);

    [Theory]
    [InlineData(0f, -100f, CrossingDirection.Forward)]
    [InlineData(50f, -100f, CrossingDirection.Forward)]
    [InlineData(0f, 100f, CrossingDirection.Backward)]
    [InlineData(100f, 0f, CrossingDirection.Forward)]
    public void Classify_ComparesVelocityWithRaceDirection(float vx, float vy, CrossingDirection expected) =>
        Assert.Equal(expected, FinishLineCrossing.Classify(new Vector2(vx, vy), RaceDirection));
}
