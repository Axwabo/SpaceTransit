# SpaceTransit

A tra(i)nsit simulator game made in Unity, set in a fictional space base.

Ships travel in fixed tubes, as if they were train tracks.
They arrive at docks (platforms) at designated stations. 

The transit system is inspired by real-world railway systems,
with significant modifications to fit the theme and also to simplify code.
It's resilient and auto-configured, making it easy to add stations in the editor.

Several FLASH and ROSS ships serve numerous stations based on timetables.
When a ship has no routes to take, it will go to a ship house.

# Gameplay

Ships are automatically controlled by default. The player can choose to manually "drive" a ship.

The COSMOS system ensures safety of ships.
If the player chooses to take control, they must respect the COSMOS signals.
Overriding automatic actions performed by the ship can lead to catastrophic effects.

The game will end upon a ship crashing as the system is designed to prevent such conditions.

# Acronyms

Ships' acronyms were inspired by [Stadler](https://stadlerrail.com/) trains.
Acronyms in parentheses represent the source of inspiration.

- `COSMOS` (ETCS) = control-oriented safety measurement overseeing system
- `FLASH` (FLIRT) = fast, light, advanced space hauler
- `ROSS` (KISS) = rapid, optimized space shuttle
