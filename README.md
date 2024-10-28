# Chess DOS Project

A simple, command-line chess game built for DOS environments. 

## Features
- **Play Chess**: Engage in a standard chess game through command-line inputs.
- **Turn-Based Gameplay**: The game alternates between White and Black turns, following classic chess rules.
- **Check/Checkmate Detection**: Game logic includes basic check and checkmate detection.

## Getting Started

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/DavidZalewski/chess.git
   ```
2. **Run the Program**:
   - Execute in a compatible DOS environment.

## Example Gameplay

```plaintext
Chess Application Starting
Would you like to play the tutorial? (y/n):
(input) n 
Select RuleSet:
*Classic* -- The game we all know and love
*PawnsOnly* -- No knights, bishops, rooks, or queens. Just a king and his pawns
*NuclearHorse* -- The game where the most powerful piece is also the most destructive, can you survive the environment?
*SevenByEight* -- The board is changed to be a 7 x 8 grid instead
*KingsForce* -- A pawn connected to a king can capture in all directions; the king can disable a square
(input) Classic 
*Adding RuleSet: Classic
Applying Rule Sets
Use AI for Black? (y, n)?
(input) n 
*|*A*|*B*|*C*|*D*|*E*|*F*|*G*|*H*|*
8|BR1|BN1|BB1|BQ1|BK |BB2|BN2|BR2|8
7|BP1|BP2|BP3|BP4|BP5|BP6|BP7|BP8|8
6|   |   |   |   |   |   |   |   |8
5|   |   |   |   |   |   |   |   |8
4|   |   |   |   |   |   |   |   |8
3|   |   |   |   |   |   |   |   |8
2|WP1|WP2|WP3|WP4|WP5|WP6|WP7|WP8|8
1|WR1|WN1|WB1|WQ1|WK |WB2|WN2|WR2|8
*|*A*|*B*|*C*|*D*|*E*|*F*|*G*|*H*|*

Turn 1 - White to move. Please enter a command (piece name + position : ie. 'WK1 C3')
(input) 
```

Here’s an explanation for each piece's name based on the console output:

- **WK**: White King - the most important piece, which must be protected.
- **WQ**: White Queen - powerful piece that can move any number of squares in any direction.
- **WR**: White Rook - moves vertically or horizontally across the board.
- **WB**: White Bishop - moves diagonally any number of squares.
- **WN**: White Knight - moves in an L-shape (two squares in one direction and one square perpendicular).
- **WP**: White Pawn - moves forward one square but captures diagonally; can move two squares forward on its first move.
- **BK**: Black King - same as the White King, crucial to protect.
- **BQ**: Black Queen - same abilities as the White Queen.
- **BR**: Black Rook - same as the White Rook.
- **BB**: Black Bishop - same as the White Bishop.
- **BN**: Black Knight - same as the White Knight.
- **BP**: Black Pawn - same as the White Pawn.

The numbered pieces indicate multiple instances of the same type (e.g., **WR1** and **WR2** for White Rooks).


## Project Setup
The entire solution consists of 4 projects:
Chess -> The core chess engine written in C#
Chess.comReplayETL -> The chess.com ETL that feeds full replays from chess.com into this engine to be verified. Written in python.
Prototypes -> An unused project that contains prototype code.
Tests -> C# Unit Tests that test the functionality of the chess engine. There are over 400 unit tests in this project.

Not all features are finished yet. For example the ChessStateExplorer is incomplete. It suffers from severe performance issues and has been turned off during automated testing.

This project also contains a custom build command that is integrated into Visual Studio. `Build Test CORE` command builds all Tests annotated with [CORE] attribute, and output test reports and logs from each run for analysis.

## Contributions

Feel free to submit issues, pull requests, or suggestions!