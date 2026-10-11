using Godot;
using HovercraftRace.Core.Physics;

namespace HovercraftRace.Game.Vehicles;

/// <summary>Reads the <c>{prefix}_up/down/left/right</c> actions from the project's input map.</summary>
public sealed class KeyboardDriveInput(string actionPrefix) : IDriveInputSource
{
    private readonly StringName _up = $"{actionPrefix}_up";
    private readonly StringName _down = $"{actionPrefix}_down";
    private readonly StringName _left = $"{actionPrefix}_left";
    private readonly StringName _right = $"{actionPrefix}_right";

    public DriveInput Read() => new(
        Accelerate: Input.IsActionPressed(_up),
        Brake: Input.IsActionPressed(_down),
        Steer: Input.GetAxis(_left, _right));
}
