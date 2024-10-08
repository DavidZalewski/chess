import requests
import json

# Replace this with your Chess.com username and the date range you want
username = "7oneseven"
year = "2024"
month = "10"

# Chess.com API URL for fetching games
url = f"https://api.chess.com/pub/player/{username}/games/{year}/{month}"

# Send a GET request to the API
response = requests.get(url)

# Check if the request was successful
if response.status_code == 200:
    games_data = response.json()
    
    # Loop through all games
    for game in games_data['games']:
        if 'pgn' in game:
            pgn_data = game['pgn']
            print("PGN Data:")
            print(pgn_data)  # Print the PGN of the game
            
            # Optionally save the PGN to a file for later processing
            with open(f"pgn\{username}_game_{game['end_time']}.pgn", "w") as pgn_file:
                pgn_file.write(pgn_data)
else:
    print(f"Failed to retrieve games. Status code: {response.status_code}")