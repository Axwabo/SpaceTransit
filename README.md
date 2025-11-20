# SpaceTransit

> [!IMPORTANT]
> I really rushed to get the game to a playable state in a week,
> but I underestimated how much time it would take.
> 
> Updates coming soon™ :3

A tra(i)nsit simulator game made in Unity, set in a fictional space country.

Ships travel in fixed tubes, as if they were on train tracks.
They arrive at docks (platforms) at designated stations. 

The transit system is inspired by real-world railway systems,
with significant modifications to fit the theme and also to simplify code.
It's dynamic, making it easy to add stations in the Unity editor.

Several FLASH and ROSS ships serve numerous stations based on timetables.
Currently, only 1 line has been built.

~~When a ship has no routes to take, it will go to a ship house.~~ (not yet implemented)

# Gameplay

> [!IMPORTANT]
> If the world doesn't load, or you fall out, press `Tab` to (re)open the menu, click `Exit`
> to go back to the main menu and try again.

Ships are automatically controlled by default. The player can choose to manually "drive" a ship.

The COSMOS ensures ship safety.
If the player chooses to take control, they must respect the COSMOS signals.
Overriding automatic actions performed by the ship can lead to catastrophic effects.

~~The game will end upon a ship crashing as the system is designed to prevent such conditions.~~
not yet implemented

# Acronyms

Ships' acronyms were inspired by [Stadler](https://stadlerrail.com/) trains.
Acronyms in parentheses represent the source of inspiration.

- `COSMOS` (ETCS) = control-oriented safety measurement overseeing system
- `FLASH` (FLIRT) = fast, light, advanced space hauler
- `ROSS` (KISS) = rapid, optimized space shuttle

# References

`Vaulter` route controller system name inspiration: [Vultron](https://www.vultron.hu/)

All station names have been inspired by a station in the real world.
Can you find them all?

<small>There's a hint somewhere in the repository</small>

# Unity

The best game engine... only crashed about 10 times on average every day :SteamHappy:

Since the camera's near plane cannot be too small (below 0.01), I had to make the world about 10 m/unit.
Due to floating-point limitations, objects start to flicker immensely.

To combat this, the world is moved in the opposite way when the player or the ship moves.

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
