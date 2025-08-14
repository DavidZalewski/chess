# NOTE

This project uses stockfish for learning purposes. The source code and original license is distributed with this project. If the developers of stockfish have an issue with the way their code has been distributed, please feel free to reach out to me here on GitHub, and I will change it.

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

# Code Journey

## Project Journey Story: The Evolution of David Zalewski's Chess Game

### Chapter 1: The Beginning

**Commit 1 - Dec 27, 2023**: The chess project embarked on its journey with a simple yet significant initial commit. This foundational step set the stage for what would become an intricate system capable of simulating the classic game of chess. The first commit was a blank canvas, a starting point filled with potential and promise.

### Chapter 2: Growing Pains

**Commit 14 - Dec 31, 2023**: As the year drew to a close, the project underwent its first major refactor. This was a pivotal moment, a chance to clean up early design flaws and lay a more robust framework for future development. It was a transformative process, streamlining code and enhancing clarity.

**Commit 16 - Jan 2, 2024**: Entering the new year, the focus shifted to implementing complex game mechanics. Pawn promotion was introduced, factory methods were added, and the initial logic for the king piece began to take shape. These updates marked an exciting phase of progress and innovation.

### Chapter 3: Refinement and Progress

**Commit 19 - Jan 3, 2024**: Another round of significant refactoring brought with it the KingCheckService, a crucial component for ensuring valid game states. This was followed by a reorganization of files, improving the project's structure and maintainability.

**Commit 21 - Jan 3, 2024**: The team celebrated "Good Progress!!!", a testament to the strides made in enhancing the game's functionality and design. This optimism fueled further development, pushing the project closer to its vision.

### Chapter 4: Building Complexity

**Commit 32 - Mar 6, 2024**: As spring approached, the focus returned to the chessboard itself. The Board Position was refactored, clarifying and optimizing the way pieces were tracked and moved.

**Commit 34 - Mar 8, 2024**: A redesign of the ChessBoard came next, a bold move to refine the core component of the game. This redesign laid the groundwork for a more intuitive and efficient gameplay experience.

### Chapter 5: Achieving Functionality

**Commit 50 - Mar 14, 2024**: By mid-March, the game was 98% functional. This milestone reflected the team's dedication and hard work, with only minor tweaks remaining before a fully operational game could be declared.

**Commit 53 - Mar 15, 2024**: The following day, the game reached full functionality. This was a momentous occasion, a culmination of months of effort and a testament to the team's commitment to excellence.

### Chapter 6: Enhancements and Challenges

**Commit 60 - Jun 6, 2024**: Development continued with enhancements like the ultrafast count of depth7 possible moves, pushing the boundaries of what the chess engine could achieve.

**Commit 71 - Jul 8, 2024**: The introduction of NuclearHorse code added a new layer of complexity, opening doors to advanced strategies and gameplay dynamics.

### Chapter 7: Testing and Perfection

**Commit 76 - Oct 6, 2024**: With the game mechanics solidified, attention turned to testing. A custom build test tool was developed to ensure the game's robustness and reliability.

**Commit 78 - Oct 7, 2024**: Challenges such as the failing Caesars Mate Test were addressed, demonstrating the team's resilience and problem-solving skills.

**Commit 88 - Oct 21, 2024**: The notorious En Passant Double Capture Bug was finally conquered, a victory for the developers and a smoother experience for players.

### Chapter 8: Closing the Loop

**Commit 89 - Oct 25, 2024**: All chess replay files were verified, ensuring consistency and accuracy in game simulations.

**Commit 97 - Oct 28, 2024**: Features like stalemate and resignation handling were implemented, rounding out the game's rule set and providing a more complete gameplay experience.

### Chapter 9: Final Touches

**Commit 111 - Nov 3, 2024** and **Commit 120 - Nov 7, 2024**: As the project neared completion, final touches were made, including updates to the GameControllerTests. These final steps ensured that the game was not only functional but polished and ready for players to enjoy.

This journey from inception to completion reflects the dedication, creativity, and perseverance of the development team. Each commit tells a story of challenges overcome and milestones achieved, culminating in a chess game that is both a technical achievement and a joy to play.
