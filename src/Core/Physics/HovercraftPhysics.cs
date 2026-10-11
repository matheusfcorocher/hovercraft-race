using System.Numerics;

namespace HovercraftRace.Core.Physics;

/// <summary>
/// Engine-independent hovercraft handling: a bicycle steering model with
/// speed-dependent traction, so the craft grips at low speed and drifts at high speed.
/// </summary>
public static class HovercraftPhysics
{
    /// <summary>Advances the motion by <paramref name="delta"/> seconds. Collisions are resolved by the caller.</summary>
    public static MotionState Step(MotionState state, DriveInput input, HovercraftSettings settings, float delta)
    {
        var velocity = state.Velocity;
        var acceleration = EngineAcceleration(state.Heading, input, settings);

        if (velocity.Length() < settings.StopSpeed)
            velocity = Vector2.Zero;
        acceleration += Resistance(velocity, settings);

        var steered = Steer(velocity, state.Heading, input.ClampedSteer, settings, delta);
        return steered with { Velocity = steered.Velocity + acceleration * delta };
    }

    private static Vector2 EngineAcceleration(Vector2 heading, DriveInput input, HovercraftSettings settings)
    {
        if (input.Brake)
            return heading * settings.Braking;
        if (input.Accelerate)
            return heading * settings.EnginePower;
        return Vector2.Zero;
    }

    private static Vector2 Resistance(Vector2 velocity, HovercraftSettings settings) =>
        velocity * settings.Friction + velocity * velocity.Length() * settings.Drag;

    private static MotionState Steer(Vector2 velocity, Vector2 heading, float steer, HovercraftSettings settings, float delta)
    {
        var steerAngle = steer * settings.SteeringAngleDegrees * MathF.PI / 180f;
        var halfBase = heading * settings.WheelBase / 2f;

        var rearWheel = -halfBase + velocity * delta;
        var frontWheel = halfBase + Rotate(velocity, steerAngle) * delta;
        var newHeading = Vector2.Normalize(frontWheel - rearWheel);

        var speed = velocity.Length();
        if (speed == 0f)
            return new MotionState(velocity, newHeading);

        var alignment = Vector2.Dot(newHeading, velocity / speed);
        var traction = speed > settings.SlipSpeed ? settings.TractionFast : settings.TractionSlow;

        if (alignment > 0f)
            velocity = Vector2.Lerp(velocity, newHeading * speed, traction);
        else if (alignment < 0f)
            velocity = -newHeading * MathF.Min(speed, settings.MaxReverseSpeed);

        return new MotionState(velocity, newHeading);
    }

    private static Vector2 Rotate(Vector2 v, float radians)
    {
        var (sin, cos) = MathF.SinCos(radians);
        return new Vector2(v.X * cos - v.Y * sin, v.X * sin + v.Y * cos);
    }
}
