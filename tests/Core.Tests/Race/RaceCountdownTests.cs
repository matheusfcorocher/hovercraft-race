using HovercraftRace.Core.Race;

namespace HovercraftRace.Core.Tests.Race;

public class RaceCountdownTests
{
    [Fact]
    public void Ticks_ShowCountdownThenGoThenHide()
    {
        var countdown = new RaceCountdown(from: 3);
        var shown = new List<string> { countdown.Display };

        for (var i = 0; i < 4; i++)
        {
            countdown.Tick();
            shown.Add(countdown.Display);
        }

        Assert.Equal(["3", "2", "1", RaceCountdown.GoText, ""], shown);
        Assert.Equal(CountdownPhase.Finished, countdown.Phase);
    }

    [Fact]
    public void RaceStarts_WhenGoAppears()
    {
        var countdown = new RaceCountdown(from: 2);
        var started = 0;
        countdown.RaceStarted += () => started++;

        countdown.Tick();
        Assert.False(countdown.HasRaceStarted);

        countdown.Tick();
        Assert.True(countdown.HasRaceStarted);
        Assert.Equal(1, started);
    }

    [Fact]
    public void Finished_RaisedOnceAfterGo()
    {
        var countdown = new RaceCountdown(from: 1);
        var finished = 0;
        countdown.Finished += () => finished++;

        for (var i = 0; i < 5; i++)
            countdown.Tick();

        Assert.Equal(1, finished);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void From_MustBePositive(int from) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new RaceCountdown(from));
}
