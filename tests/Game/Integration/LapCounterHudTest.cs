using System.Threading.Tasks;
using GdUnit4;
using Godot;
using HovercraftRace.Core.Race;
using HovercraftRace.Game.UI;
using HovercraftRace.Game.Vehicles;
using HovercraftRace.Tests.Game.Support;
using static GdUnit4.Assertions;

namespace HovercraftRace.Tests.Game.Integration;

[TestSuite]
[RequireGodotRuntime]
public class LapCounterHudTest
{
    [TestCase]
    public async Task ShowsLapsOfBoundRacer()
    {
        var root = new Node();
        var hud = Scenes.Instantiate<LapCounterHud>(Scenes.LapCounterHud);
        var craft = Scenes.Instantiate<Hovercraft>(Scenes.HovercraftRed);
        craft.InputSource = new ScriptedDriveInput(ScriptedDriveInput.Idle);
        root.AddChild(hud);
        root.AddChild(craft);
        var runner = ISceneRunner.Load(root, autoFree: true);
        await runner.AwaitIdleFrame();

        hud.Bind(craft);
        AssertThat(hud.LapsLabel!.Text).IsEqual("0");

        craft.CrossFinishLine(CrossingDirection.Forward);
        craft.CrossFinishLine(CrossingDirection.Forward);

        AssertThat(hud.LapsLabel.Text).IsEqual("2");
    }
}
