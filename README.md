# Boogle Game
---

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
   - Use `dotnet run` if .NET8 is installed on your machine if you're not using any IDE
   - For debugging purpose use `dotnet run --property:OutputType=Exe` which forces terminal outputs
5. A prebuild version will be released at the end of the project.

## Usage
1. Play the game
2. Once the game starts, the game board will be displayed, and the first player will have 1 minute to find as many words as possible.
3. The player types in the words they have found, and the game will validate them against the dictionary and calculate the score.
4. After the first player's turn, the next player will have their turn, and so on until the time limit is reached.
5. Enjoy
   
## Implementation Details
1. For the User Interface, the solution will be a **Winform**.
2. Game
3. Board -> Dice / Dictionary
4. Trie
5. Graph

## Path of work
### Chapter 1 : What's the problem ?
Before starting anything, let's see what we needed to do as for this project.

Recreating the famous boggle by implementing a board which possesses a fixed number of dices, each one having 6 faces with one letter. We will consider in this project a ***non-fixed*** number of letters but a ***weigth based system*** instead.
This will be a turn based game with a determined time that each player holds at the start of the game.

[Thanks to Kaimira for providing an open source weighted list in C#.](https://github.com/cdanek/KaimiraWeightedList)

An essential concept of a the game is the need to validate whether or not an inputted word truly exist on the board. But how ? By searching every paths of course, however everyone knows that it will be way to slow to just brute force the solution. Something that we thought was just easier to do was by abandoning *"useless"* paths, which means prefixes with non existant word within itself. (i.e. In `[Maison, Banane]`, the prefix `Bn` will be abandoned because there is are no words with the prefix `Bn`, in contrary to `Ma` in `Maison`)

### Chapter 2 : Downsides of mandatory and required methods
For this project, our first idea was to collect all the words from given dictionaries (french and/or english) and store them in an array which needed to be sorted to use the mandatory method `RechDichoRecursif` for an element lookup in `O(log(n))` time complexity.

The problem ? Because we needed process every words and the prefixed, sorting them was in reality pointless, using too much memory and time for little to no results. For example :
```csharp
//get every words in the dictionary
string words = File.ReadAllLines(filePath)[0];
string[] wordsInArray = words.Split(' ');

//then add them to a list and try to proceed them
foreach (string word in wordArray){
   [...]
}
wordArray = wordArray.Sort() 

```
But we still didnt proceed any prefixes not their length, which is really really bad.

### Chapter 3 : Data Structure and algorithm (Trie / Graph)
That's why we came up with another solution, implementing a `Trie`, a specialized ***tree-like*** data structure that stores a ***dynamic*** set of strings, typically to facilitate fast searching, insertion, and deletion operations. It is particularly useful for managing dictionaries, auto-complete features, and ***prefix-based search***.

![Trie Data Structure Illustration](/markdownassets/Triedatastructure1.webp)

This will allow us later to keep track of used prefixes during the word search. 

And that's when graphes come into play, a `Graph` is a data structure used to represent ***pairwise relationships between objects***. A graph consists of nodes (`vertices`), which represent the objects, and edges (`arcs`), which represent the connections between these objects. We also need to implement the **adjacency matrix** of a graph connect every vertices to each other.

![Adjacency Mat Graph](/markdownassets/adjacency_mat_graph.webp)

The reasons I used a graph are because :
1) To *Learn*
2) To Facilitate a **Depth-First Search (DFS)** algorithm for a complete word search within the board.

For the implementation of the graph, we put in place a custom generic collection `MatrixGraphBuilder<T>` which turned any 2D matrix in its adjacency graph. \
 For example: The board with [A..P], connected vertically, horizontally and diagonally.

![Graph](/markdownassets/graph.png)

### Chapter 4 : The hard part, finding every possible playable words on the board
The core challenge of the Boogle game lies in identifying all the valid words that can be formed on the board using a *combination* of adjacent letters. This involves a combination of Depth-First Search (`DFS`), `Trie`, and `graph` traversal to systematically explore all possible paths and efficiently check whether they form valid words.

Since some words share prefixes, it’s **inefficient** to check all possible letter combinations blindly. Instead, we optimize this by abandoning paths where prefixes do not match any valid word in the dictionary.

```csharp
public SortedList<string, string> getAllValidWordsOnBoard(TrieNode trieNode)
{
   SortedList<string, string> sortedWordList = new();
   bool[,] visited;

   // Loop through all nodes in the graph
   foreach (Node<char> node in _graph.Graph.Keys){
      visited = new bool[_boardWidth, _boardHeight];
      DFS(visited, node, string.Empty); // Start DFS from each node
   }

   void DFS(bool[,] visited, Node<char> node, string currentPrefix){
      if (visited[node.Row, node.Col]) return;
      visited[node.Row, node.Col] = true;
      string dfsPrefix = currentPrefix + node.Value;
      
      if (!Trie.SearchPrefix(trieNode, dfsPrefix)){ // Check if the prefix exists in the trie
         visited[node.Row, node.Col] = false;
         return; // Backtrack if the prefix doesn't exist in the Trie
      }
      // If a complete word is found, add it to the result list
      if (Trie.SearchKey(trieNode, dfsPrefix)){
         sortedWordList[dfsPrefix] = dfsPrefix;
      }
      // Recursively explore all neighbors
      if (_graph.Graph.TryGetValue(node, out HashSet<Node<char>> neighbors)){
         foreach (Node<char> neighbor in neighbors)
         {
            bool[,] newVisited = (bool[,])visited.Clone(); // Avoid revisiting nodes in the same path
            DFS(newVisited, neighbor, dfsPrefix);
         }
      }
   }
   return sortedWordList;
}
```

**Detailed Explanation of the Algorithm :**
1. Graph Setup: We initialize a graph where each node corresponds to a letter on the board. An adjacency matrix is created to capture the connections between adjacent nodes (letters).

2. DFS Initialization: We start a DFS from every node on the board, treating each letter as a potential starting point for a word. At each step, we build a string representing the current word path.

3. Prefix Checking: As we build each word path, we check if the current prefix exists in the Trie. If it doesn't, the search terminates early for that path. This prevents unnecessary exploration of paths that cannot lead to a valid word.

4. Word Validation: If the current string matches a full word in the Trie, it is added to a `SortedList<string, string>` of valid words. This ensures that only words present in the dictionary are considered valid, the utilization of SortedList gives us the opportunity the use the mandatory .

## Conlusion :
This project was in fact interesting by putting us in one hand on the utilization and implementation of simple relatively simple things, like creating a board, players, looping through external `.txt` files.
On the other hand, more basic understanding of data structures and algorithms is required like **graph theory**, **set theory**, **DFS approach** and much more by letting us dive into the wild with only ***arrays*** and ***loops***.
Learning to gather unlearned informations was a key method to succeed in the project.