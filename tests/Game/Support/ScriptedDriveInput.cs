using HovercraftRace.Core.Physics;
using HovercraftRace.Game.Vehicles;

namespace HovercraftRace.Tests.Game.Support;

/// <summary>Drives a hovercraft from test code instead of the keyboard.</summary>
public sealed class ScriptedDriveInput(DriveInput input) : IDriveInputSource
{
    public static DriveInput Idle => DriveInput.None;
    public static DriveInput Forward => new(Accelerate: true, Brake: false, Steer: 0f);
    public static DriveInput Reverse => new(Accelerate: false, Brake: true, Steer: 0f);

    public DriveInput Current { get; set; } = input;

    public DriveInput Read() => Current;
}
