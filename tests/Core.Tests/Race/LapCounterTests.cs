using HovercraftRace.Core.Race;

namespace HovercraftRace.Core.Tests.Race;

public class LapCounterTests
{
    private readonly LapCounter _counter = new();
    private readonly List<int> _events = [];

    public LapCounterTests() => _counter.LapCompleted += _events.Add;

    [Fact]
    public void StartsAtZero() => Assert.Equal(0, _counter.CompletedLaps);

    [Fact]
    public void ForwardCrossing_CompletesLap()
    {
        _counter.RegisterCrossing(CrossingDirection.Forward);

        Assert.Equal(1, _counter.CompletedLaps);
        Assert.Equal([1], _events);
    }

    [Fact]
    public void ConsecutiveForwardCrossings_CountEachLap()
    {
        _counter.RegisterCrossing(CrossingDirection.Forward);
        _counter.RegisterCrossing(CrossingDirection.Forward);
        _counter.RegisterCrossing(CrossingDirection.Forward);

        Assert.Equal(3, _counter.CompletedLaps);
        Assert.Equal([1, 2, 3], _events);
    }

    [Fact]
    public void BackwardCrossing_DoesNotCount()
    {
        _counter.RegisterCrossing(CrossingDirection.Backward);

        Assert.Equal(0, _counter.CompletedLaps);
        Assert.Empty(_events);
    }

    [Fact]
    public void BackAndForthOverTheLine_EarnsNoExtraLaps()
    {
        _counter.RegisterCrossing(CrossingDirection.Forward);   // lap 1
        _counter.RegisterCrossing(CrossingDirection.Backward);  // reverse over the line
        _counter.RegisterCrossing(CrossingDirection.Forward);   // cross again: still lap 1

        Assert.Equal(1, _counter.CompletedLaps);
        Assert.Equal([1], _events);
    }

    [Fact]
    public void ReversingOutOfTheStart_MustBeMadeUpBeforeLapCounts()
    {
        _counter.RegisterCrossing(CrossingDirection.Backward);
        _counter.RegisterCrossing(CrossingDirection.Forward);

        Assert.Equal(0, _counter.CompletedLaps);

        _counter.RegisterCrossing(CrossingDirection.Forward);

        Assert.Equal(1, _counter.CompletedLaps);
    }
}
