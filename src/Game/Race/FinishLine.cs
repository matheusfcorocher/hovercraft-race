using Godot;
using HovercraftRace.Core.Race;
using HovercraftRace.Game.Vehicles;

namespace HovercraftRace.Game.Race;

/// <summary>Tells each hovercraft that drives through it which way it crossed.</summary>
[GlobalClass]
public partial class FinishLine : Area2D
{
    /// <summary>Direction of travel for a forward crossing, in the line's local space.</summary>
    [Export] public Vector2 RaceDirection { get; set; } = Vector2.Up;

    public override void _Ready() => BodyEntered += OnBodyEntered;

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Hovercraft hovercraft)
            return;

        var worldDirection = GlobalTransform.BasisXform(RaceDirection);
        hovercraft.CrossFinishLine(FinishLineCrossing.Classify(hovercraft.Velocity.ToNumerics(), worldDirection.ToNumerics()));
    }
}
