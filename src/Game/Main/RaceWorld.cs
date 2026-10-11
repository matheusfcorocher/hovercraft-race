using Godot;
using HovercraftRace.Game.Vehicles;

namespace HovercraftRace.Game.Main;

/// <summary>The playable level: track, finish line and racers.</summary>
[GlobalClass]
public partial class RaceWorld : Node2D
{
    [Export] public TileMapLayer? Track { get; set; }

    [Export] public Hovercraft? PlayerOne { get; set; }

    [Export] public Hovercraft? PlayerTwo { get; set; }
}
