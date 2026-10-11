using System.Threading.Tasks;
using GdUnit4;
using Godot;
using HovercraftRace.Core.Physics;
using HovercraftRace.Game.Vehicles;
using HovercraftRace.Tests.Game.Support;
using static GdUnit4.Assertions;

namespace HovercraftRace.Tests.Game.Integration;

[TestSuite]
[RequireGodotRuntime]
public class HovercraftTest
{
    private static (ISceneRunner Runner, Hovercraft Craft) Spawn(DriveInput input)
    {
        var craft = Scenes.Instantiate<Hovercraft>(Scenes.HovercraftRed);
        craft.InputSource = new ScriptedDriveInput(input);
        return (ISceneRunner.Load(craft, autoFree: true), craft);
    }

    [TestCase]
    public async Task Idle_StaysPut()
    {
        var (runner, craft) = Spawn(ScriptedDriveInput.Idle);

        await runner.RunFor(0.5);

        AssertThat(craft.Position).IsEqual(Vector2.Zero);
    }

    [TestCase]
    public async Task Accelerating_MovesAlongHeading()
    {
        var (runner, craft) = Spawn(ScriptedDriveInput.Forward);

        await runner.RunFor(0.5);

        AssertThat(craft.Position.X).IsGreater(20f);
        AssertThat(Mathf.Abs(craft.Position.Y)).IsLess(1f);
    }

    [TestCase]
    public async Task Accelerating_SwitchesToFastEngineAnimation()
    {
        var (runner, craft) = Spawn(ScriptedDriveInput.Forward);

        await runner.RunFor(1.5);

        AssertThat(craft.CurrentSpeedTier).IsEqual(SpeedTier.Fast);
        AssertThat(craft.Sprite!.Animation.ToString()).IsEqual("fast");
    }

    [TestCase]
    public async Task Idle_UsesSlowEngineAnimation()
    {
        var (runner, craft) = Spawn(ScriptedDriveInput.Idle);

        await runner.RunFor(0.2);

        AssertThat(craft.Sprite!.Animation.ToString()).IsEqual("slow");
    }
}
