using Godot;

namespace HovercraftRace.Tests.Game.Support;

public static class Scenes
{
    public const string Main = "res://scenes/main/Main.tscn";
    public const string HovercraftRed = "res://scenes/vehicles/HovercraftRed.tscn";
    public const string FinishLine = "res://scenes/race/FinishLine.tscn";
    public const string StartCountdown = "res://scenes/race/StartCountdown.tscn";
    public const string LapCounterHud = "res://scenes/ui/LapCounterHud.tscn";

    public static T Instantiate<T>(string path) where T : Node =>
        GD.Load<PackedScene>(path).Instantiate<T>();
}
