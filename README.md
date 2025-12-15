# SpaceTransit

A tra(i)nsit simulator game made in Unity, set in a fictional space country.

Ships travel in fixed tubes, as if they were on train tracks.
They arrive at docks (platforms) at designated stations.

The transit system is inspired by real-world railway systems,
with significant modifications to fit the theme and also to simplify code.
It's dynamic, making it fairly easy to add stations in the Unity editor.

Several FLASH and ROSS ships serve numerous stations based on timetables.
Currently, only 1 line has been built.

# Gameplay

> [!IMPORTANT]
> If the world doesn't load, or you fall out, press `Tab` to (re)open the menu, click `Exit`
> to go back to the main menu and try again.

https://github.com/user-attachments/assets/7f9684a1-b117-4cc3-a586-91d0505a9167

Ships are automatically controlled by default. The player can choose to manually "drive" a ship.

The COSMOS ensures ship safety.
If the player chooses to take control, they must respect the COSMOS signals.
~~Overriding automatic actions performed by the ship can lead to catastrophic effects.~~ not yet implemented. The ship ensures speed limits, and forcefully stops if the COSMOS system reports that it cannot proceed. The diode in the middle of the control panel indicates this.

Some station docks have multiple exits. In this scenario, one must be picked to proceed.

When entering a station with multiple docks in the same direction, a dock must be picked to enter into. Some docks may not be connected to the entry tube or may be occupied; in this case, the selection turns red.

Entries and exits may hold zero, one or multiple locks while a ship is passing through. Locked segments cannot be used by other ships.

The automatic driver locks entries and exits as soon as it's near enough.

When you're on board, audio sources outside of the current ship will be muffled.
The closer you are to an open door and the more open it is, the less muffled sounds will be.
Unfortunately, the low-pass filter [doesn't work on WebGL](https://docs.unity3d.com/Manual/webgl-audio.html)

The game will end upon a ship crashing as the system is designed to prevent such conditions.

# Acronyms

Ships' acronyms were inspired by [Stadler](https://stadlerrail.com/) trains.
Acronyms in parentheses represent the source of inspiration.

- `COSMOS` (ETCS) = control-oriented safety measurement overseeing system
- `FLASH` (FLIRT) = fast, light, advanced space hauler
- `ROSS` (KISS) = rapid, optimized space shuttle

# References

`Vaulter` route controller system name inspiration: [Vultron](https://www.vultron.hu/)

All station names have been inspired by a station in the real world.
Can you find them all? There's a hint somewhere in the repository.

# Unity

The best game engine... only crashed about 10 times on average every day :SteamHappy:

Since the camera's near plane cannot be too small (below 0.01), I had to make the world about 10 m/unit.
Due to floating-point limitations, objects start to flicker immensely.

To combat this, the world is moved in the opposite way when the player teleports or as the ship is sailing.

## SplineMesh

I used [SplineMesh](https://assetstore.unity.com/packages/tools/modeling/splinemesh-104989) to
automatically generate tubes.

To improve performance, I modified the code to use Spans (:3) in the `CubicBezierCurve` class,
and changed the `CurveSample` equality check to compare structs instead of objects.

The latter significantly reduced frame time because it prevents boxing (and therefore heap allocations).

# AI Usage

I recorded train sounds, and used [Vocal Remover](https://vocalremover.org/next/) to separate
speech and sound effects.

No SpaceTransit code was written by AI.

# Models

I made the models myself, and as someone who's barely blended (Blender) before, it's painful

Creating the new ROSS ship model took nearly 9 hours

# Resuming

You can "continue" playing at any time between 5:00:00 AM and 9:59:59 PM.

The ships are spawned according to the current time and the timetables:

1. If departing from origin later than now, spawn at origin
2. If arrived at a station and departing in more than minimum stay + 1 minute, spawn at intermediate stop
3. If arriving at a station in 1 minute, only spawn 1 minute before arrival time 2 tubes before the station
4. If (scheduled) left last stop and arriving at destination, or completed route cycle, spawn at destination
5. Otherwise, wait and keep checking step 2

For now, step 3 spawns the ship at the first possible entry (if any).
This leads to Saturplace -> Peshtadub routes arriving from the Mountypile branch at Gyuard.
The route system wasn't designed with this in mind initially, it'll require a slight rework.

# TODO

- [X] ROSS ship remodeling (to double-decker)
- [ ] More realistic ROSS audio
- [X] Map view
- [X] Route and timetable list in menu
- [X] Better controls display
- [X] Interactive tutorial (or at least pages)
- [X] Ship selection in test driving scene
- [ ] Many more routes and lines (half-done?)
- [X] Support for multiple entries per dock
- [X] Control panel improvements
- [X] Resuming at any time
- [ ] Support midnight reset
- [X] Automatic ship route rotation

# Notes/Issues

- Exit list might not show
- Lower-end devices will most likely face issues with lag, therefore overruns
- Sometimes, the ship might overrun the dock; in this case you need to restart, e.g.
    - Starting at Mountypile, 09:59:00 causes 46 to overrun Cárpenter

Future plans:

- Dynamic line loading
- Ship houses
- Even more routes & lines
- Spline-based, auto-generated map
- Touchscreen support
- Ship summoning
