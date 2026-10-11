using HovercraftRace.Core.Physics;

namespace HovercraftRace.Core.Tests.Physics;

public class SpeedTierTests
{
    [Theory]
    [InlineData(0f, SpeedTier.Slow)]
    [InlineData(SpeedTiers.MediumFrom, SpeedTier.Slow)]
    [InlineData(SpeedTiers.MediumFrom + 1f, SpeedTier.Medium)]
    [InlineData(SpeedTiers.FastFrom, SpeedTier.Medium)]
    [InlineData(SpeedTiers.FastFrom + 1f, SpeedTier.Fast)]
    public void Classify_UsesSpeedBands(float speed, SpeedTier expected) =>
        Assert.Equal(expected, SpeedTiers.Classify(speed));
}
