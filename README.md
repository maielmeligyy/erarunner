# EraRunner

A 2D endless runner game where players travel through different historical eras, facing obstacles, solving puzzles, and collecting powerups.

## Game Concept

In EraRunner, players log in to access a fast-paced endless runner where they:

1. Run through historical eras (Ancient Egypt, Medieval, Industrial Revolution, Future, etc.)
2. Dodge era-specific obstacles that get faster as the score increases
3. Collect items that trigger era-transition puzzles
4. Solve mini-puzzles to earn powerups before entering the next era
5. Maintain a high score that increases faster the longer they survive

## Game Features

- Login/signup system with username and password (currently test/1234)
- Multiple historical eras with unique visual themes
- Increasingly difficult endless runner gameplay
- Score system that saves the player's high score
- Four mini-puzzle types between eras:
  - Memory match
  - Quick-time events
  - Simple history quizzes
  - Pattern sequences
- Powerup system with various bonuses:
  - Shield (temporary invulnerability)
  - Speed boost
  - Score multiplier
  - Extra lives
- Lives system with UI heart icons

## Controls

- Tap or click to jump over obstacles
- Interactive puzzles between eras

## Technical Implementation

The game is built with several core systems:

- **GameManager**: Controls game flow, score, eras, and overall game state
- **PlayerMovement**: Handles player controls, collision detection, and lives
- **PuzzleManager**: Implements the four different puzzle types between eras
- **PowerupManager**: Manages different powerup effects and their durations
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