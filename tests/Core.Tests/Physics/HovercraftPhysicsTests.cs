using System.Numerics;
using HovercraftRace.Core.Physics;

namespace HovercraftRace.Core.Tests.Physics;

public class HovercraftPhysicsTests
{
    private const float Delta = 1f / 60f;
    private static readonly HovercraftSettings Settings = HovercraftSettings.Default;
    private static readonly MotionState FacingRight = MotionState.AtRest(0f);

    private static MotionState Simulate(MotionState state, DriveInput input, float seconds)
    {
        for (var t = 0f; t < seconds; t += Delta)
            state = HovercraftPhysics.Step(state, input, Settings, Delta);
        return state;
    }

    [Fact]
    public void NoInput_AtRest_StaysAtRest()
    {
        var state = Simulate(FacingRight, DriveInput.None, seconds: 1f);

        Assert.Equal(Vector2.Zero, state.Velocity);
        Assert.Equal(FacingRight.Heading, state.Heading);
    }

    [Fact]
    public void Accelerate_FromRest_GainsSpeedAlongHeading()
    {
        var state = HovercraftPhysics.Step(FacingRight, new DriveInput(Accelerate: true, Brake: false, Steer: 0), Settings, Delta);

        Assert.Equal(Settings.EnginePower * Delta, state.Velocity.X, precision: 3);
        Assert.Equal(0f, state.Velocity.Y, precision: 3);
    }

    [Fact]
    public void Accelerate_Held_ConvergesToTerminalSpeed()
    {
        // Terminal speed solves EnginePower = -Friction * v - Drag * v².
        var a = -Settings.Drag;
        var b = -Settings.Friction;
        var terminal = (-b + MathF.Sqrt(b * b + 4 * a * Settings.EnginePower)) / (2 * a);

        var state = Simulate(FacingRight, new DriveInput(true, false, 0), seconds: 20f);

        Assert.InRange(state.Speed, terminal - 2f, terminal + 2f);
    }

    [Fact]
    public void Coasting_EventuallyComesToRest()
    {
        var moving = FacingRight with { Velocity = new Vector2(300f, 0f) };

        var state = Simulate(moving, DriveInput.None, seconds: 10f);

        Assert.Equal(Vector2.Zero, state.Velocity);
    }

    [Fact]
    public void Brake_FromRest_ReversesWithoutTurningAround()
    {
        var state = Simulate(FacingRight, new DriveInput(false, true, 0), seconds: 1f);

        Assert.True(state.Velocity.X < 0f, $"expected backwards motion, got {state.Velocity}");
        Assert.True(Vector2.Dot(state.Heading, FacingRight.Heading) > 0.99f, $"heading flipped to {state.Heading}");
    }

    [Fact]
    public void Brake_Held_ReverseSpeedIsCapped()
    {
        var state = Simulate(FacingRight, new DriveInput(false, true, 0), seconds: 10f);

        var oneStepOfBraking = MathF.Abs(Settings.Braking) * Delta;
        Assert.InRange(state.Speed, 0f, Settings.MaxReverseSpeed + oneStepOfBraking);
    }

    [Fact]
    public void BothPedals_BrakeWins()
    {
        var both = HovercraftPhysics.Step(FacingRight, new DriveInput(true, true, 0), Settings, Delta);
        var brake = HovercraftPhysics.Step(FacingRight, new DriveInput(false, true, 0), Settings, Delta);

        Assert.Equal(brake, both);
    }

    [Theory]
    [InlineData(1f, 1)]   // Godot's Y axis points down, so steering right turns clockwise (+Y).
    [InlineData(-1f, -1)]
    public void Steer_WhileMoving_TurnsTowardsSteerDirection(float steer, int expectedSign)
    {
        var moving = FacingRight with { Velocity = new Vector2(200f, 0f) };

        var state = HovercraftPhysics.Step(moving, new DriveInput(false, false, steer), Settings, Delta);

        Assert.Equal(expectedSign, MathF.Sign(state.Heading.Y));
    }

    [Fact]
    public void Steer_AtRest_DoesNotTurn()
    {
        var state = HovercraftPhysics.Step(FacingRight, new DriveInput(false, false, 1f), Settings, Delta);

        Assert.Equal(FacingRight.Heading, state.Heading);
    }

    [Fact]
    public void Steer_BeyondFullLock_IsClamped()
    {
        var moving = FacingRight with { Velocity = new Vector2(200f, 0f) };

        var overSteer = HovercraftPhysics.Step(moving, new DriveInput(false, false, 5f), Settings, Delta);
        var fullLock = HovercraftPhysics.Step(moving, new DriveInput(false, false, 1f), Settings, Delta);

        Assert.Equal(fullLock, overSteer);
    }

    [Fact]
    public void Steer_AboveSlipSpeed_DriftsMoreThanBelow()
    {
        static float SlipAngle(float speed)
        {
            var moving = FacingRight with { Velocity = new Vector2(speed, 0f) };
            var state = HovercraftPhysics.Step(moving, new DriveInput(false, false, 1f), Settings, Delta);
            return MathF.Acos(Math.Clamp(Vector2.Dot(Vector2.Normalize(state.Velocity), state.Heading), -1f, 1f));
        }

        Assert.True(SlipAngle(Settings.SlipSpeed + 50f) > SlipAngle(Settings.SlipSpeed - 50f));
    }

    [Fact]
    public void Rotation_MatchesHeadingAngle()
    {
        var state = MotionState.AtRest(MathF.PI / 2f);

        Assert.Equal(MathF.PI / 2f, state.Rotation, precision: 5);
    }
}
