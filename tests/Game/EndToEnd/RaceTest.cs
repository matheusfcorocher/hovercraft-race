using System.Threading.Tasks;
using GdUnit4;
using Godot;
using HovercraftRace.Game.Main;
using HovercraftRace.Game.Race;
using HovercraftRace.Game.UI;
using HovercraftRace.Tests.Game.Support;
using static GdUnit4.Assertions;

namespace HovercraftRace.Tests.Game.EndToEnd;

/// <summary>Plays the real Main scene through the input map, like a player at the keyboard.</summary>
[TestSuite]
[RequireGodotRuntime]
public class RaceTest
{
    private ISceneRunner _runner = null!;
    private Node _main = null!;
    private RaceWorld _world = null!;
    private StartCountdown _countdown = null!;

    [BeforeTest]
    public void LoadRace()
    {
        _main = Scenes.Instantiate<Node>(Scenes.Main);
        _countdown = _main.GetNode<StartCountdown>("StartCountdown");
        _countdown.StepSeconds = 0.2f;
        _runner = ISceneRunner.Load(_main, autoFree: true);
        _world = _main.GetNode<RaceWorld>("Simulation/RaceWorld");
    }

    [AfterTest]
    public void Cleanup() => Simulation.Unpause();

    private async Task WaitForGo() =>
        await ISceneRunner.AwaitSignalOn(_countdown, StartCountdown.SignalName.RaceStarted).WithTimeout(3000);

    private string HudText(string view) =>
        _main.GetNode<LapCounterHud>($"Views/{view}/Hud").LapsLabel!.Text;

    [TestCase]
    public async Task Racers_HoldStillDuringCountdown_ThenDriveOnGo()
    {
        var start = _world.PlayerOne!.Position;

        _runner.SimulateActionPress("p1_up");
        await _runner.RunFor(0.3);
        AssertThat(_world.PlayerOne.Position).IsEqual(start);

        await WaitForGo();
        await _runner.RunFor(0.5);
        _runner.SimulateActionRelease("p1_up");

        AssertThat(_world.PlayerOne.Position.Y).IsLess(start.Y - 20f);
    }

    [TestCase]
    public async Task PlayerTwo_DrivesWithWasd()
    {
        await WaitForGo();
        var start = _world.PlayerTwo!.Position;
        var playerOneStart = _world.PlayerOne!.Position;

        _runner.SimulateKeyPress(Key.W);
        await _runner.RunFor(0.5);
        _runner.SimulateKeyRelease(Key.W);

        AssertThat(_world.PlayerTwo.Position.Y).IsLess(start.Y - 20f);
        AssertThat(_world.PlayerOne.Position).IsEqual(playerOneStart);
    }

    [TestCase]
    public async Task CrossingFinishLine_UpdatesOnlyThatPlayersHud()
    {
        await WaitForGo();
        _world.PlayerOne!.Position = new Vector2(214, 1030);

        _runner.SimulateActionPress("p1_up");
        await _runner.RunFor(1.0);
        _runner.SimulateActionRelease("p1_up");

        AssertThat(_world.PlayerOne.Position.Y).IsLess(940f);
        AssertThat(HudText("RightView")).IsEqual("1");
        AssertThat(HudText("LeftView")).IsEqual("0");
    }

    [TestCase]
    public async Task RiverBank_StopsTheRacer()
    {
        const float RightBankX = 256f;
        await WaitForGo();
        _world.PlayerOne!.Rotation = 0f;

        _runner.SimulateActionPress("p1_up");
        await _runner.RunFor(1.5);
        _runner.SimulateActionRelease("p1_up");

        AssertThat(_world.PlayerOne.Position.X).IsLess(RightBankX);
    }
}
