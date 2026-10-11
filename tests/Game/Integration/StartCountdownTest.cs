using System.Threading.Tasks;
using GdUnit4;
using Godot;
using HovercraftRace.Core.Race;
using HovercraftRace.Game.Race;
using HovercraftRace.Tests.Game.Support;
using static GdUnit4.Assertions;

namespace HovercraftRace.Tests.Game.Integration;

[TestSuite]
[RequireGodotRuntime]
public class StartCountdownTest
{
    [AfterTest]
    public void Cleanup() => Simulation.Unpause();

    [TestCase]
    public async Task PausesUntilGo_ThenHides()
    {
        var countdown = Scenes.Instantiate<StartCountdown>(Scenes.StartCountdown);
        countdown.From = 2;
        countdown.StepSeconds = 0.1f;
        var runner = ISceneRunner.Load(countdown, autoFree: true);
        var tree = countdown.GetTree();

        AssertThat(tree.Paused).IsTrue();
        AssertThat(countdown.Label!.Text).IsEqual("2");

        await ISceneRunner.AwaitSignalOn(countdown, StartCountdown.SignalName.RaceStarted).WithTimeout(2000);

        AssertThat(tree.Paused).IsFalse();
        AssertThat(countdown.Label.Text).IsEqual(RaceCountdown.GoText);

        await runner.RunFor(0.3);

        AssertThat(countdown.Visible).IsFalse();
    }
}
