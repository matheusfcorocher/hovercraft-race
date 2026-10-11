namespace HovercraftRace.Core.Physics;

/// <summary>Coarse speed bands used to pick the engine animation.</summary>
public enum SpeedTier
{
    Slow,
    Medium,
    Fast,
}

public static class SpeedTiers
{
    public const float MediumFrom = 50f;
    public const float FastFrom = 300f;

    public static SpeedTier Classify(float speed) => speed switch
    {
        > FastFrom => SpeedTier.Fast,
        > MediumFrom => SpeedTier.Medium,
        _ => SpeedTier.Slow,
    };
}
