# Battle Tanks

A 2D top-down tank combat game inspired by the NES classic *Battle City* (Namco, 1985), built in Unity 6 as my graduation thesis at the Faculty of Electrical Engineering, University of Belgrade.

The player defends a base (the eagle) against waves of enemy tanks across 20 levels, destroying terrain and collecting power-ups along the way.

## Features

- Five terrain types with different behavior: brick (destructible), steel, water, ice, trees
- Four enemy tank types (Basic, Fast, Power, Armor) with simple AI
- Six power-ups: Grenade, Helmet, Shovel, Star, Tank, Timer
- 20 levels with a three-tier difficulty progression
- One or two player mode
- Three difficulty modes: Easy, Hard, Hardcore
- Per-player scoring, end-of-level tally, persistent high score
- Sound effects and music with separate volume settings
- Custom level editor tool for the Unity Editor

## Controls

| Action | Player 1 | Player 2 |
|---|---|---|
| Move | W A S D | Arrow keys |
| Fire | Space | Enter |
| Pause | Esc | Esc |

## Running the game

**From source:**
1. Install Unity **[exact version, see `ProjectSettings/ProjectVersion.txt`]**
2. Clone this repository and open the folder as a project in Unity Hub
3. The first import regenerates the `Library/` folder, which is not tracked in Git, so it may take a few minutes
4. Open the `MainMenu` scene and press Play

## Project structure

```
Assets/
├── Scripts/     Gameplay code (managers, player, enemies, bullets, power-ups)
├── Editor/      Level editor window (Editor-only)
├── Levels/      LevelData assets, one per level
├── Tiles/       Tile assets and tile palette
├── Sprites/     Tank, terrain, UI and icon sprites
├── Audio/       Sound effects and music
└── Scenes/      MainMenu and GameScene
```

Key scripts: `GameManager`, `StageManager`, `LevelLoader`, `LevelData`, `EnemySpawner`, `ScoreManager`, `PowerUpManager`, `AudioManager`, `PlayerController`, `EnemyTank`, `Bullet`.

## Level editor

Levels are stored as `LevelData` ScriptableObjects. To edit them in Unity, open **Battle Tanks → Level Editor**, assign a `LevelData` asset, and paint terrain or enemy spawn points on the grid. The border, the base and its surrounding bricks are fixed and cannot be edited.

## Credits

- Sound effects and music from [Freesound](https://freesound.org) and [Kenney](https://kenney.nl). The full attribution list is in the in-game Credits panel.
- Font: [Orbitron](https://fonts.google.com/specimen/Orbitron) (Google Fonts)
- Sprites were generated with AI assistance. All code, architecture and level design are my own work.

## About

Graduation thesis, Faculty of Electrical Engineering, University of Belgrade, 2026.
Author: Ozren Jovanović · Mentor: Marko Mišić
