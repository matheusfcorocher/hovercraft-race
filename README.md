# hovercraft-race
The game is about a race between two hovercrafts and was made with <a href="https://godotengine.org/">Godot Game Engine</a>.
The objective of it was to attend a required project of Digital Entertainment class in college. It was inspired in event that occurred in <a href="https://www.eep.br/">EEP</a>.

The original GDScript/Godot 3 version was ported to **Godot 4.7 with C#**.

## Features
<dl>
  <dt>Split-Screen</dt>
  <dd>
    Spit-Screen is common technique used in game development for two players play together in the same screen. In hovercraft-race, it divides vertically the screen.
  </dd>
  <dt>Minimap</dt>
  <dd>An overview of the whole track at the bottom of the screen.</dd>
  <dt>Lap counter</dt>
  <dd>Each player's laps are shown in their half of the screen. Driving backwards over the finish line does not count.</dd>
</dl>

## Preview
<img src="https://user-images.githubusercontent.com/56937223/204866806-2cdb7069-73a2-4a6e-a38c-173fb6d3941a.png" width="100%"></img>

## Requirements
- [Godot 4.7 **.NET** edition](https://godotengine.org/download) (the standard edition cannot run C#)
- [.NET SDK 10](https://dotnet.microsoft.com/download)

### Linux / WSL
```bash
mkdir -p ~/.local/opt && cd ~/.local/opt
curl -LO https://github.com/godotengine/godot/releases/download/4.7.2-stable/Godot_v4.7.2-stable_mono_linux_x86_64.zip
unzip Godot_v4.7.2-stable_mono_linux_x86_64.zip && rm Godot_v4.7.2-stable_mono_linux_x86_64.zip
ln -sf ~/.local/opt/Godot_v4.7.2-stable_mono_linux_x86_64/Godot_v4.7.2-stable_mono_linux.x86_64 ~/.local/bin/godot

# ~/.bashrc or ~/.zshrc
export PATH="$HOME/.local/bin:$PATH"
export GODOT_BIN="$HOME/.local/bin/godot"   # used by the Godot tests
```
On WSL the window opens through WSLg. If it starts black, add `--rendering-driver opengl3`.

## Quick start
```bash
godot --path .        # play
godot -e --path .     # open the editor (then press F5)
```

## Controls
The game only can be played with keyboard.

| | Player Red (right screen) | Player Green (left screen) |
|---|---|---|
| Forward | ↑ | W |
| Backward | ↓ | S |
| Turn left | ← | A |
| Turn right | → | D |

## Tests
```bash
dotnet test tests/Core.Tests                            # unit: physics, laps, countdown (no Godot needed)
dotnet test HovercraftRace.csproj --settings .runsettings  # integration + end-to-end inside Godot (needs GODOT_BIN)
```
- **Unit** (`tests/Core.Tests`, xUnit): the engine-independent rules.
- **Integration** (`tests/Game/Integration`, gdUnit4): single scenes, e.g. a hovercraft driving through the finish line.
- **End-to-end** (`tests/Game/EndToEnd`, gdUnit4): the real `Main.tscn` driven through the input map: countdown, both players' keys, lap HUD, track collisions.

## Project structure
```
src/Core/        Game rules in plain C# (no Godot): hovercraft physics, lap counting, start countdown
src/Game/        Godot nodes that adapt Core to the engine
  Vehicles/        Hovercraft (CharacterBody2D), input sources, tuning resource
  Race/            FinishLine, StartCountdown
  Cameras/         FollowCamera (split screen), OverviewCamera (minimap)
  UI/              LapCounterHud
  Main/            RaceWorld (level), PlayerView/MinimapView, SplitScreenRace (composition root)
scenes/          .tscn files, one folder per area (mirrors src/Game)
assets/          sprites, tilesets, fonts, ui
tests/           Core.Tests (xUnit) and Game (gdUnit4)
```
The world is simulated once in an off-screen `SubViewport`; each player view and the minimap render that same `World2D` through their own camera.

## Credits
Track tiles from the Liberated Pixel Cup, licensed CC-BY-SA 3.0 / GPL 3.0. See [assets/tilesets/ATTRIBUTION.txt](assets/tilesets/ATTRIBUTION.txt).
