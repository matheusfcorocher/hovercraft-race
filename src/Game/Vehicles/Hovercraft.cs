using Godot;
using HovercraftRace.Core.Physics;
using HovercraftRace.Core.Race;

namespace HovercraftRace.Game.Vehicles;

/// <summary>A racer: feeds driver input through <see cref="HovercraftPhysics"/> and lets Godot resolve collisions.</summary>
[GlobalClass]
public partial class Hovercraft : CharacterBody2D
{
    [Signal]
    public delegate void LapCompletedEventHandler(int laps);

    /// <summary>Input map prefix, e.g. <c>p1</c> reads <c>p1_up</c>, <c>p1_left</c>...</summary>
    [Export] public string InputPrefix { get; set; } = "p1";

    [Export] public HovercraftTuning? Tuning { get; set; }

    [Export] public AnimatedSprite2D? Sprite { get; set; }

    /// <summary>Defaults to the keyboard for <see cref="InputPrefix"/>; replace it to drive the craft from code.</summary>
    public IDriveInputSource? InputSource { get; set; }

    public LapCounter Laps { get; } = new();

    public SpeedTier CurrentSpeedTier { get; private set; }

    private HovercraftSettings _settings = HovercraftSettings.Default;

    public override void _Ready()
    {
        MotionMode = MotionModeEnum.Floating;
        _settings = Tuning?.ToSettings() ?? HovercraftSettings.Default;
        InputSource ??= new KeyboardDriveInput(InputPrefix);
        Laps.LapCompleted += laps => EmitSignal(SignalName.LapCompleted, laps);
    }

    public override void _PhysicsProcess(double delta)
    {
        var current = MotionState.AtRest(Rotation) with { Velocity = Velocity.ToNumerics() };
        var next = HovercraftPhysics.Step(current, InputSource!.Read(), _settings, (float)delta);

        Rotation = next.Rotation;
        Velocity = next.Velocity.ToGodot();
        MoveAndSlide();

        UpdateAnimation();
    }

    public void CrossFinishLine(CrossingDirection direction) => Laps.RegisterCrossing(direction);

    private void UpdateAnimation()
    {
        CurrentSpeedTier = SpeedTiers.Classify(Velocity.Length());
        Sprite?.Play(AnimationFor(CurrentSpeedTier));
    }

    private static StringName AnimationFor(SpeedTier tier) => tier switch
    {
        SpeedTier.Fast => Animations.Fast,
        SpeedTier.Medium => Animations.Medium,
        _ => Animations.Slow,
    };

    public static class Animations
    {
        public static readonly StringName Slow = "slow";
        public static readonly StringName Medium = "medium";
        public static readonly StringName Fast = "fast";
    }
}
