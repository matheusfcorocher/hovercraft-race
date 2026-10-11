namespace HovercraftRace.Core.Race;

/// <summary>
/// Counts laps from finish-line crossings. Backward crossings undo forward ones,
/// so driving back and forth over the line never earns a lap.
/// </summary>
public sealed class LapCounter
{
    private int _netCrossings;

    public int CompletedLaps { get; private set; }

    /// <summary>Raised with the new lap total.</summary>
    public event Action<int>? LapCompleted;

    public void RegisterCrossing(CrossingDirection direction)
    {
        _netCrossings += direction == CrossingDirection.Forward ? 1 : -1;
        if (_netCrossings <= CompletedLaps)
            return;

        CompletedLaps = _netCrossings;
        LapCompleted?.Invoke(CompletedLaps);
    }
}
