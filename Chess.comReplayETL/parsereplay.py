import chess.pgn

def parse_pgn_file(pgn_file_path):
    # Load the PGN file
    with open(pgn_file_path) as pgn_file:
        game = chess.pgn.read_game(pgn_file)

    # Extract moves
    board = game.board()
    moves = []
    for move in game.mainline_moves():
        board.push(move)
        moves.append(str(move))  # Capture the move in UCI format, e.g., 'e2e4'

    return moves

# Example usage
pgn_file_path = "yourusername_game_1672531200.pgn"
moves = parse_pgn_file(pgn_file_path)
print("Extracted Moves:", moves)

# You can now convert these moves into commands for your chess engine.