using HovercraftRace.Core.Physics;

namespace HovercraftRace.Game.Vehicles;

/// <summary>Where a hovercraft gets its driver intent: a keyboard, a test script, an AI...</summary>
public interface IDriveInputSource
{
    DriveInput Read();
}
