# EraRunner

A 2D endless runner game where players travel through different historical eras, facing obstacles, solving puzzles, and collecting powerups.

## Game Concept

In EraRunner, players log in to access a fast-paced endless runner where they:

1. Run through historical eras (Stone era, Ancient Egypt, and more can be added in future enhancements etc.)
3. Jump era-specific obstacles that get faster as the score increases
4. Collect coins that increase score
5. Solve mini-riddles to earn powerups before entering the next era

## Game Features

- Login/signup system with username and password 
- Multiple historical eras with unique visual themes
- an era-related riddle between eras:
- Powerup system with various bonuses:
  - Shield (temporary invulnerability)
  - Extra lives
- Lives system with UI heart icons

## Controls

- Tap or click the arrow to jump over obstacles
- Interactive riddle between eras

## Technical Implementation

The game is built with several core systems:

- **GameManager**: Controls game flow, score, eras, and overall game state
- **PlayerMovement**: Handles player controls, collision detection, and lives
- **PuzzleManager**: Implements the four different puzzle types between eras - not fully impleemented in game
- **PowerupManager**: Manages different powerup effects and their durations - not fully implemented in game
- **ObstacleGenerator**: Dynamically spawns obstacles based on the current era
- **EndlessGround**: Creates the scrolling ground effect with infinite terrain

## Setup Instructions

1. Clone this repository
2. Open the project in Unity (recommended version: 2021.3 or newer)
3. Open the LoginScene
4. Press Play to test the game
5. Log in with username "test" and password "1234"

## Future Enhancements

- More detailed era-specific backgrounds and obstacles
- Animated character with multiple animation states
- Sound effects and era-appropriate music
- More complex puzzles related to each historical era
- Online leaderboard for high scores
- More powerup types and special abilities

## Credits

Built using Unity and free assets from the Unity Asset Store. 
