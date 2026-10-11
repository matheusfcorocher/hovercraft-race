namespace HovercraftRace.Core.Physics;

/// <summary>
/// Tuning values for the hovercraft's arcade "bicycle model" handling.
/// Defaults reproduce the original GDScript tuning.
/// </summary>
public sealed record HovercraftSettings
{
    /// <summary>Distance between the virtual front and rear wheels. Larger turns wider.</summary>
    public float WheelBase { get; init; } = 70f;

    public float SteeringAngleDegrees { get; init; } = 50f;

    public float EnginePower { get; init; } = 800f;

    /// <summary>Acceleration while braking or reversing. Negative: pushes backwards.</summary>
    public float Braking { get; init; } = -450f;

    /// <summary>Linear resistance, dominant at low speed. Negative: opposes velocity.</summary>
    public float Friction { get; init; } = -0.9f;

    /// <summary>Quadratic resistance, dominant at high speed. Negative: opposes velocity.</summary>
    public float Drag { get; init; } = -0.001f;

    public float MaxReverseSpeed { get; init; } = 250f;

    /// <summary>Above this speed traction drops to <see cref="TractionFast"/> and the craft drifts.</summary>
    public float SlipSpeed { get; init; } = 400f;

    /// <summary>How fast velocity aligns with the heading (0..1) when drifting.</summary>
    public float TractionFast { get; init; } = 0.1f;

    /// <summary>How fast velocity aligns with the heading (0..1) below <see cref="SlipSpeed"/>.</summary>
    public float TractionSlow { get; init; } = 0.7f;

    /// <summary>Speeds below this snap to zero so the craft comes to rest.</summary>
    public float StopSpeed { get; init; } = 5f;

    public static HovercraftSettings Default { get; } = new();
}
