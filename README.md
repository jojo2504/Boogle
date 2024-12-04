# Boogle Game

## Summary
- [Introduction](https://github.com/jojo2504/Boogle?tab=readme-ov-file#introduction)
- [Features (TODO)](https://github.com/jojo2504/Boogle?tab=readme-ov-file#features-todo)
- [Features (DONE)](https://github.com/jojo2504/Boogle?tab=readme-ov-file#features-done)
- [Getting Started (For Custom Building)](https://github.com/jojo2504/Boogle?tab=readme-ov-file#getting-started-for-custom-building)
- [Usage](https://github.com/jojo2504/Boogle?tab=readme-ov-file#usage)
- [Implementation Details](https://github.com/jojo2504/Boogle?tab=readme-ov-file#implementation-details)
- [Path of work](https://github.com/jojo2504/Boogle?tab=readme-ov-file#path-of-work)
- [Known Bugs](https://github.com/jojo2504/Boogle?tab=readme-ov-file#known-bugs)

## Introduction
Boogle is a word game where players try to find as many words as possible from a grid of randomly generated letters within a given time limit. The game is designed to be played by two or more players, and the player with the highest score at the end of the game is declared the winner.
### This repository is a project for A2 students in computer sciences.

## Features (TODO)
- Customizable game settings, including the size of the game board and the duration of the game.
- Calculates the score of each word based on the point values of the letters and the length of the word.
- Implement the game logic.
- Displays the final scores and the winner.
- Generates a word cloud for each player's found words.
- Usable user interface to play the game.

## Features (DONE)
- Supports multiple languages (French and English by default).
- Maintains a list of words found by each player.
- Implement algorithm to find every possible playable words on the current board formed by the dices.

## Getting Started (For Custom Building)
1. Clone the repository to your local machine.
2. Open the solution in your IDE (i.e. VSCode, Rider or Visual Studio).
3. Build the solution to ensure that all dependencies are satisfied.
4. Run the project to start the game
   - Use ```dotnet run``` if .NET8 is installed on your machine if you're not using any IDE
   - For debugging purpose use ```dotnet run --property:OutputType=Exe``` which forces terminal outputs
5. A prebuild version will be released at the end of the project.

## Usage
1. Play the game
2. Once the game starts, the game board will be displayed, and the first player will have 1 minute to find as many words as possible.
3. The player types in the words they have found, and the game will validate them against the dictionary and calculate the score.
4. After the first player's turn, the next player will have their turn, and so on until the time limit is reached.
5. Enjoy
   
## Implementation Details
For the User Interface, the solution will be a **Winform**.

## Path of work

## Known Bugs

## Bonus
The game includes a bonus feature where the player is an AI that explores all possible positions on the game board to find words in the dictionary. This implementation uses optimization techniques to minimize the search time, such as abandoning search paths that lead to prefixes that do not exist in the dictionary.
