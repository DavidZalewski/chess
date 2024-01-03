using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class GameController
    {
        private ChessBoard _chessBoard;
        private List<ChessPiece> _chessPieces = new();
        private List<Turn> _turns = new();

        public GameController(ChessBoard chessBoard) 
        {
            this._chessBoard = chessBoard;
            _chessPieces = ChessPieceFactory.CreateChessPieces();
        }

        public void StartGame()
        {
            _chessBoard.PopulateBoard(_chessPieces);
        }

        public void ParseMove(String consoleInput)
        {

        }

    }
}
