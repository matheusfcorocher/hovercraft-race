using System.Threading.Tasks;
using GdUnit4;
using Godot;
using HovercraftRace.Core.Physics;
using HovercraftRace.Game.Race;
using HovercraftRace.Game.Vehicles;
using HovercraftRace.Tests.Game.Support;
using static GdUnit4.Assertions;

namespace HovercraftRace.Tests.Game.Integration;

[TestSuite]
[RequireGodotRuntime]
public class FinishLineTest
{
    /// <summary>A finish line at the origin (race direction: up) and a craft facing up at <paramref name="start"/>.</summary>
    private static (ISceneRunner Runner, Hovercraft Craft) Arena(Vector2 start, DriveInput input)
    {
        var root = new Node2D();
        root.AddChild(Scenes.Instantiate<FinishLine>(Scenes.FinishLine));

        var craft = Scenes.Instantiate<Hovercraft>(Scenes.HovercraftRed);
        craft.Position = start;
        craft.Rotation = -Mathf.Pi / 2;
        craft.InputSource = new ScriptedDriveInput(input);
        root.AddChild(craft);

        return (ISceneRunner.Load(root, autoFree: true), craft);
    }

    [TestCase]
    public async Task DrivingThroughForward_CompletesLapAndEmitsSignal()
    {
        var (runner, craft) = Arena(new Vector2(0, 80), ScriptedDriveInput.Forward);
        AssertSignal(craft).StartMonitoring();

        await runner.RunFor(1.0);

        AssertThat(craft.Position.Y).IsLess(-60f);
        AssertThat(craft.Laps.CompletedLaps).IsEqual(1);
        await AssertSignal(craft).IsEmitted(Hovercraft.SignalName.LapCompleted, 1).WithTimeout(100);
    }

    [TestCase]
    public async Task ReversingThrough_DoesNotCountLap()
    {
        var (runner, craft) = Arena(new Vector2(0, -80), ScriptedDriveInput.Reverse);

        await runner.RunFor(1.5);

        AssertThat(craft.Position.Y).IsGreater(60f);
        AssertThat(craft.Laps.CompletedLaps).IsEqual(0);
    }
}
