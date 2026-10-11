using Godot;

namespace HovercraftRace.Game.Cameras;

/// <summary>Tracks a node that may live in another viewport sharing the same <see cref="World2D"/>.</summary>
[GlobalClass]
public partial class FollowCamera : Camera2D
{
    [Export] public Node2D? Target { get; set; }

    /// <summary>Keeps framing the racer while the tree is paused (e.g. during the start countdown).</summary>
    public override void _Ready() => ProcessMode = ProcessModeEnum.Always;

    public override void _PhysicsProcess(double delta)
    {
        if (Target is not null)
            GlobalPosition = Target.GlobalPosition;
    }
}
