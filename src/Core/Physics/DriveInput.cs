namespace HovercraftRace.Core.Physics;

/// <summary>Driver intent for one physics step.</summary>
/// <param name="Accelerate">Engine on, pushing along the heading.</param>
/// <param name="Brake">Brake/reverse. Wins over <paramref name="Accelerate"/> when both are held.</param>
/// <param name="Steer">-1 (full left) to 1 (full right).</param>
public readonly record struct DriveInput(bool Accelerate, bool Brake, float Steer)
{
    public static DriveInput None => default;

    public float ClampedSteer => Math.Clamp(Steer, -1f, 1f);
}
