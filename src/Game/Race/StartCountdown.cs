using Godot;
using HovercraftRace.Core.Race;

namespace HovercraftRace.Game.Race;

/// <summary>Holds the race paused while showing the <see cref="RaceCountdown"/>.</summary>
[GlobalClass]
public partial class StartCountdown : CanvasLayer
{
    [Signal]
    public delegate void RaceStartedEventHandler();

    [Export] public Label? Label { get; set; }

    [Export(PropertyHint.Range, "1,10")] public int From { get; set; } = 3;

    [Export(PropertyHint.Range, "0.1,5,0.1,suffix:s")] public float StepSeconds { get; set; } = 1f;

    public RaceCountdown Countdown { get; private set; } = new();

    private readonly Timer _timer = new();

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        Countdown = new RaceCountdown(From);
        Countdown.RaceStarted += OnRaceStarted;
        Countdown.Finished += OnFinished;

        GetTree().Paused = true;
        ShowCurrent();

        _timer.WaitTime = StepSeconds;
        _timer.Timeout += OnTick;
        AddChild(_timer);
        _timer.Start();
    }

    private void OnTick()
    {
        Countdown.Tick();
        ShowCurrent();
    }

    private void OnRaceStarted()
    {
        GetTree().Paused = false;
        EmitSignal(SignalName.RaceStarted);
    }

    private void OnFinished()
    {
        _timer.Stop();
        Hide();
    }

    private void ShowCurrent()
    {
        if (Label is not null)
            Label.Text = Countdown.Display;
    }
}
