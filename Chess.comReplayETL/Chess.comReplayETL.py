import json
import os
import requests
import chess.pgn

# Load the configuration from the JSON file
with open("etl_config.json") as config_file:
    config = json.load(config_file)

# Extract config values
pgn_directory = config["pgn_directory"]
converted_moves_directory = config["converted_moves_directory"]
username = config["chess_com_username"]
year = config["chess_com_year"]
month = config["chess_com_month"]

# Ensure directories exist
os.makedirs(pgn_directory, exist_ok=True)
os.makedirs(converted_moves_directory, exist_ok=True)

# Chess.com API URL for fetching games
url = f"https://api.chess.com/pub/player/{username}/games/{year}/{month}"

print(url)

# Add headers, including a User-Agent to mimic a browser request
headers = {
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36"
}

# Send the GET request with headers
response = requests.get(url, headers=headers)

# Check if the request was successful
if response.status_code == 200:
    games_data = response.json()
    
    # Loop through all games
    for game in games_data['games']:
        if 'pgn' in game:
            pgn_data = game['pgn']
            
            # Save the PGN to the pgn directory
            with open(os.path.join(pgn_directory, f"{username}_game_{game['end_time']}.pgn"), "w") as pgn_file:
                pgn_file.write(pgn_data)
else:
    print(f"Failed to retrieve games. Status code: {response.status_code}")
    exit(-1)


# Initialize piece trackers for white and black
white_pawns = {}
black_pawns = {}
white_knights = {}
black_knights = {}
white_bishops = {}
black_bishops = {}
white_rooks = {}
black_rooks = {}
white_king = "WK"
black_king = "BK"
white_queen = "WQ1"
black_queen = "BQ1"

# Initialize piece tracking based on starting positions
def initialize_piece_tracking():
    global white_pawns, black_pawns, white_knights, black_knights
    global white_bishops, black_bishops, white_rooks, black_rooks

    # Initialize white pawns on rank 2 (files a-h)
    white_pawns = {(1, i): f"WP{i+1}" for i in range(8)}  # Pawns at a2, b2, ..., h2

    # Initialize black pawns on rank 7 (files a-h)
    black_pawns = {(6, i): f"BP{i+1}" for i in range(8)}  # Pawns at a7, b7, ..., h7

    # Initialize white knights at b1 (1, 1) and g1 (1, 6)
    white_knights = {(0, 1): "WK1", (0, 6): "WK2"}  # Knights at b1, g1

    # Initialize black knights at b8 (7, 1) and g8 (7, 6)
    black_knights = {(7, 1): "BK1", (7, 6): "BK2"}  # Knights at b8, g8

    # Initialize white bishops at c1 (0, 2) and f1 (0, 5)
    white_bishops = {(0, 2): "WB1", (0, 5): "WB2"}  # Bishops at c1, f1

    # Initialize black bishops at c8 (7, 2) and f8 (7, 5)
    black_bishops = {(7, 2): "BB1", (7, 5): "BB2"}  # Bishops at c8, f8

    # Initialize white rooks at a1 (0, 0) and h1 (0, 7)
    white_rooks = {(0, 0): "WR1", (0, 7): "WR2"}  # Rooks at a1, h1

    # Initialize black rooks at a8 (7, 0) and h8 (7, 7)
    black_rooks = {(7, 0): "BR1", (7, 7): "BR2"}  # Rooks at a8, h8
    
# Get the piece identifier based on its original position
def get_piece_label(piece, move):
    # Rank first, then File
    position_index = (chess.square_rank(move.from_square), chess.square_file(move.from_square))
    if piece.color == chess.WHITE:
        if piece.piece_type == chess.PAWN:
            return white_pawns.get(position_index)
        elif piece.piece_type == chess.KNIGHT:
            return white_knights.get(position_index)
        elif piece.piece_type == chess.BISHOP:
            return white_bishops.get(position_index)
        elif piece.piece_type == chess.ROOK:
            return white_rooks.get(position_index)
        elif piece.piece_type == chess.KING:
            return white_king
        elif piece.piece_type == chess.QUEEN:
            return white_queen if white_queen == "WQ1" else "WQ2"
    else:  # Black pieces
        if piece.piece_type == chess.PAWN:
            return black_pawns.get(position_index)
        elif piece.piece_type == chess.KNIGHT:
            return black_knights.get(position_index)
        elif piece.piece_type == chess.BISHOP:
            return black_bishops.get(position_index)
        elif piece.piece_type == chess.ROOK:
            return black_rooks.get(position_index)
        elif piece.piece_type == chess.KING:
            return black_king
        elif piece.piece_type == chess.QUEEN:
            return black_queen if black_queen == "BQ1" else "BQ2"
    return None  # Return None if the piece label cannot be found

# Update the piece tracker when a piece moves
def update_piece_tracking(move, piece):
    position_index = (chess.square_rank(move.from_square), chess.square_file(move.from_square))
    new_position_index = (chess.square_rank(move.to_square), chess.square_file(move.to_square))

    if piece.color == chess.WHITE:
        if piece.piece_type == chess.PAWN:
            if position_index in white_pawns:
                white_pawns[new_position_index] = white_pawns.pop(position_index)
        elif piece.piece_type == chess.KNIGHT:
            if position_index in white_knights:
                white_knights[new_position_index] = white_knights.pop(position_index)
        elif piece.piece_type == chess.BISHOP:
            if position_index in white_bishops:
                white_bishops[new_position_index] = white_bishops.pop(position_index)
        elif piece.piece_type == chess.ROOK:
            if position_index in white_rooks:
                white_rooks[new_position_index] = white_rooks.pop(position_index)
    else:  # Black pieces
        if piece.piece_type == chess.PAWN:
            if position_index in black_pawns:
                black_pawns[new_position_index] = black_pawns.pop(position_index)
        elif piece.piece_type == chess.KNIGHT:
            if position_index in black_knights:
                black_knights[new_position_index] = black_knights.pop(position_index)
        elif piece.piece_type == chess.BISHOP:
            if position_index in black_bishops:
                black_bishops[new_position_index] = black_bishops.pop(position_index)
        elif piece.piece_type == chess.ROOK:
            if position_index in black_rooks:
                black_rooks[new_position_index] = black_rooks.pop(position_index)

# Function to process and convert a PGN file (remaining the same as your original function)
def convert_pgn_file(pgn_filepath, output_filepath):
    initialize_piece_tracking()

    # Open the PGN file
    with open(pgn_filepath) as pgn_file:
        game = chess.pgn.read_game(pgn_file)
    
    # If no game found, skip the file
    if game is None:
        print(f"No valid game found in {pgn_filepath}")
        return
    
    board = game.board()
    converted_moves = []

    # Extract and convert moves
    for move in game.mainline_moves():
        piece = board.piece_at(move.from_square)  # Get the piece on the from_square
        piece_name = get_piece_label(piece, move)  # Get the piece's unique label
        from_square = chess.square_name(move.from_square)  # Convert to standard notation (e.g., e2)
        to_square = chess.square_name(move.to_square)  # Convert to standard notation (e.g., e4)

        # Handle pawn promotion
        if piece.piece_type == chess.PAWN and (chess.square_rank(move.to_square) == 0 or chess.square_rank(move.to_square) == 7):
            if piece.color == chess.WHITE:
                white_queen = "WQ2"  # Update to the new queen label
            else:
                black_queen = "BQ2"  # Update to the new queen label

        # Store the converted move in a structured format
        converted_moves.append(f"Piece: {piece_name}, From: {from_square}, To: {to_square}")

        update_piece_tracking(move, piece)  # Update piece tracking for next moves
        board.push(move)  # Make the move on the board to update the position

    # Check the final state of the game
    result = game.headers.get("Result", "Unknown")  # Check if the result is stored in the PGN file
    
    if board.is_checkmate():
        if board.turn:  # If it's White's turn, that means Black delivered checkmate
            outcome = "Black wins by checkmate"
        else:
            outcome = "White wins by checkmate"
    elif board.is_stalemate():
        outcome = "Game ended in stalemate"
    elif result == "1-0":
        outcome = "White won by resignation or other means"
    elif result == "0-1":
        outcome = "Black won by resignation or other means"
    elif result == "1/2-1/2":
        outcome = "Game ended in a draw"
    else:
        outcome = "Unknown outcome"

    # Append the game outcome to the converted moves
    converted_moves.append(f"Game outcome: {outcome}")

    # Write the converted moves to the output file
    with open(output_filepath, "w") as output_file:
        for move in converted_moves:
            output_file.write(move + "\n")

# Iterate through each PGN file in the directory
for filename in os.listdir(pgn_directory):
    if filename.endswith(".pgn"):
        pgn_filepath = os.path.join(pgn_directory, filename)
        output_filepath = os.path.join(converted_moves_directory, f"converted_{filename}")
        
        print(f"Converting {filename}...")
        convert_pgn_file(pgn_filepath, output_filepath)
        print(f"Saved converted moves to {output_filepath}")
