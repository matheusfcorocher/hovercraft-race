using Godot;
using HovercraftRace.Core.Physics;

namespace HovercraftRace.Game.Vehicles;

/// <summary>Inspector-editable wrapper around <see cref="HovercraftSettings"/>.</summary>
[GlobalClass]
public partial class HovercraftTuning : Resource
{
    private static readonly HovercraftSettings Defaults = HovercraftSettings.Default;

    [Export] public float WheelBase { get; set; } = Defaults.WheelBase;
    [Export] public float SteeringAngleDegrees { get; set; } = Defaults.SteeringAngleDegrees;
    [Export] public float EnginePower { get; set; } = Defaults.EnginePower;
    [Export] public float Braking { get; set; } = Defaults.Braking;
    [Export] public float Friction { get; set; } = Defaults.Friction;
    [Export] public float Drag { get; set; } = Defaults.Drag;
    [Export] public float MaxReverseSpeed { get; set; } = Defaults.MaxReverseSpeed;
    [Export] public float SlipSpeed { get; set; } = Defaults.SlipSpeed;
    [Export(PropertyHint.Range, "0,1")] public float TractionFast { get; set; } = Defaults.TractionFast;
    [Export(PropertyHint.Range, "0,1")] public float TractionSlow { get; set; } = Defaults.TractionSlow;
    [Export] public float StopSpeed { get; set; } = Defaults.StopSpeed;

    public HovercraftSettings ToSettings() => new()
    {
        WheelBase = WheelBase,
        SteeringAngleDegrees = SteeringAngleDegrees,
        EnginePower = EnginePower,
        Braking = Braking,
        Friction = Friction,
        Drag = Drag,
        MaxReverseSpeed = MaxReverseSpeed,
        SlipSpeed = SlipSpeed,
        TractionFast = TractionFast,
        TractionSlow = TractionSlow,
        StopSpeed = StopSpeed,
    };
}
