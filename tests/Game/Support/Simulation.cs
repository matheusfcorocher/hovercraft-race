using System.Threading.Tasks;
using GdUnit4;
using Godot;

namespace HovercraftRace.Tests.Game.Support;

public static class Simulation
{
    private const uint FrameMillis = 16;

    /// <summary>Runs roughly <paramref name="seconds"/> of game time, frame-paced so physics ticks keep up.</summary>
    public static Task RunFor(this ISceneRunner runner, double seconds) =>
        runner.SimulateFrames((uint)Mathf.CeilToInt(seconds * 1000 / FrameMillis), FrameMillis);

    /// <summary>Tests that pause the tree must not leak that state into the next test.</summary>
    public static void Unpause() => ((SceneTree)Engine.GetMainLoop()).Paused = false;
}
