using System.Globalization;

namespace HovercraftRace.Core.Race;

public enum CountdownPhase
{
    Counting,
    Go,
    Finished,
}

/// <summary>
/// Start sequence driven by an external clock: N, ..., 1, "GO!", then hidden.
/// The race starts when "GO!" appears.
/// </summary>
public sealed class RaceCountdown
{
    public const string GoText = "GO!";

    private int _remaining;

    public RaceCountdown(int from = 3)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(from, 1);
        _remaining = from;
    }

    public CountdownPhase Phase { get; private set; } = CountdownPhase.Counting;

    public bool HasRaceStarted => Phase != CountdownPhase.Counting;

    public string Display => Phase switch
    {
        CountdownPhase.Counting => _remaining.ToString(CultureInfo.InvariantCulture),
        CountdownPhase.Go => GoText,
        _ => string.Empty,
    };

    public event Action? RaceStarted;

    public event Action? Finished;

    public void Tick()
    {
        switch (Phase)
        {
            case CountdownPhase.Counting when --_remaining == 0:
                Phase = CountdownPhase.Go;
                RaceStarted?.Invoke();
                break;
            case CountdownPhase.Go:
                Phase = CountdownPhase.Finished;
                Finished?.Invoke();
                break;
        }
    }
}
