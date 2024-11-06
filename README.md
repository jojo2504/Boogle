# Boogle Game

## Introduction
Boogle is a word game where players try to find as many words as possible from a grid of randomly generated letters within a given time limit. The game is designed to be played by two or more players, and the player with the highest score at the end of the game is declared the winner.

## Features (TODO)
- Customizable game settings, including the size of the game board and the duration of the game.
- Supports multiple languages (French and English by default).
- Calculates the score of each word based on the point values of the letters and the length of the word.
- Maintains a list of words found by each player and displays the final scores and the winner.
- Generates a word cloud for each player's found words.
- Usable user interface to play the game.

## Getting Started
1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Build the solution to ensure that all dependencies are satisfied.
4. Run the project to start the game.

## Usage
1. When the game starts, you will be prompted to choose the game settings, including the size of the game board, the language, and the player names.
2. Once the game starts, the game board will be displayed, and the first player will have 1 minute to find as many words as possible.
3. The player types in the words they have found, and the game will validate them against the dictionary and calculate the score.
4. After the first player's turn, the next player will have their turn, and so on until the time limit is reached.
5. At the end of the game, the final scores and the winner are displayed, and a word cloud is generated for each player's found words.

## Implementation Details
Not implemented yet

## Bonus
The game includes a bonus feature where the player is an AI that explores all possible positions on the game board to find words in the dictionary. This implementation uses optimization techniques to minimize the search time, such as abandoning search paths that lead to prefixes that do not exist in the dictionary.

## Conclusion
Boogle is a fun and challenging word game that showcases the use of object-oriented programming principles and algorithms in C#. The game can be further extended and customized to include additional features and improvements.
