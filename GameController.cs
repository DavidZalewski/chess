using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Pieces;
using Chess.Services;
using Chess.Board;

namespace Chess
{
    public class GameController
    {
        private ChessBoard _chessBoard;
        private KingCheckService _kingCheckService;
        private List<ChessPiece> _chessPieces = new();
        private List<Turn> _turns = new();
        private int _turnNumber = 0;

        public int TurnNumber { get => _turnNumber; }

        public GameController(ChessBoard chessBoard) 
        {
            this._chessBoard = chessBoard;
            _chessPieces = ChessPieceFactory.CreateChessPieces();
            this._kingCheckService = new KingCheckService(_chessPieces);
        }

        public void StartGame()
        {
            _chessBoard.PopulateBoard(_chessPieces);
            _turnNumber = 1;
        }

        public void ParseMove(String consoleInput)
        {

        }

        internal ChessPiece FindChessPieceFromString(string input)
        {
            ChessPiece.Color color;
            ChessPiece.Piece piece;
            int id = 0;

            char[] chars = input.ToCharArray();

            if (chars.Length != 2 && chars.Length != 3)
                return null;

            switch (chars[0])
            {
                case 'W':
                    {
                        color = ChessPiece.Color.WHITE;
                        break;
                    }
                case 'B':
                    {
                        color = ChessPiece.Color.BLACK;
                        break;
                    }
                default:
                    {
                        return null;
                    }
            }

            switch (chars[1])
            {
                case 'P':
                    {
                        piece = ChessPiece.Piece.PAWN;
                        break;
                    }
                case 'K':
                    {
                        if (chars.Length == 3)
                            piece = ChessPiece.Piece.KNIGHT;
                        else if (chars.Length == 2)
                        {
                            piece = ChessPiece.Piece.KING;
                            id = 1;
                        }
                        else
                            return null;
                        break;
                    }
                case 'B':
                    {
                        piece = ChessPiece.Piece.BISHOP;
                        break;
                    }
                case 'R':
                    {
                        piece = ChessPiece.Piece.ROOK;
                        break;
                    }
                case 'Q':
                    {
                        piece = ChessPiece.Piece.QUEEN;
                        break;
                    }
                default: return null;
            }

            if (chars.Length == 3)
                if (!int.TryParse(chars[2].ToString(), out id))
                    return null;

            ChessPiece chessPiece = _chessPieces.First(p => p.GetColor().Equals(color) && p.GetPiece().Equals(piece) && p.GetId().Equals(id));

            return chessPiece;
        }

        public bool IsCheck(Turn turn)
        {
            return _kingCheckService.IsKingInCheck(turn);
        }

        public bool IsCheckMate(Turn turn)
        {
            return _kingCheckService.IsCheckMate(turn);
        }

        public Turn GetTurnFromCommand(string input)
        {
            String[] inputs = input.Split(' ');
            if (inputs.Length != 2) return null;
            try
            {
                ChessPiece chessPiece = FindChessPieceFromString(inputs[0]);

                if (chessPiece == null) return null;

                Turn turn = new(_turnNumber, chessPiece, new(inputs[1]), _chessBoard);
                return turn;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public ChessBoard GetChessBoard() { return _chessBoard; }

        public void UpdateBoard(ChessBoard board)
        {
            _chessBoard = board;
            _chessPieces = _chessBoard.PruneCapturedPieces(_chessPieces);
        }

        public string DisplayBoard()
        {
            int[,] boardData = _chessBoard.GetBoard();
            String output = "*|*A*|*B*|*C*|*D*|*E*|*F*|*G*|*H*|*\n";
            int vertIndex = 8;

            for (int f = 0; f < 8; f++)
            {
                output += vertIndex;
                for (int s = 0; s < 8; s++)
                {
                    BoardPosition boardPosition = new((BoardPosition.VERTICAL)f, (BoardPosition.HORIZONTAL)s);
                    if (_chessBoard.IsPieceAtPosition(boardPosition))
                    {
                        ChessPiece chessPiece = _chessPieces.First(p => p.GetCurrentPosition().EqualTo(boardPosition));
                        if (chessPiece == null)
                        {
                            throw new Exception("Unexpected! Fix this");
                        }
                        else
                        {
                            String c = "", p = "", i = "";
                            switch (chessPiece.GetColor())
                            {
                                case ChessPiece.Color.WHITE: c = "W"; break;
                                case ChessPiece.Color.BLACK: c = "B"; break;
                            }

                            switch (chessPiece.GetPiece())
                            {
                                case ChessPiece.Piece.PAWN: p = "P"; break;
                                case ChessPiece.Piece.KNIGHT: p = "K"; break;
                                case ChessPiece.Piece.BISHOP: p = "B"; break;
                                case ChessPiece.Piece.ROOK: p = "R"; break;
                                case ChessPiece.Piece.QUEEN: p = "Q"; break;
                                case ChessPiece.Piece.KING:
                                    {
                                        p = "K"; i = ""; break;
                                    }
                            }

                            if (!chessPiece.GetPiece().Equals(ChessPiece.Piece.KING))
                            {
                                i = chessPiece.GetId().ToString();
                            }
                            else
                            {
                                i = "0";
                            }

                            output += "|" + c + p + i;
                        }
                    }
                    else
                    {
                        output += "|   ";
                    }
                }
                output += "|" + vertIndex + "\n";
                vertIndex--;
            }
            output += "*|*A*|*B*|*C*|*D*|*E*|*F*|*G*|*H*|*\n"; 
            return output;
        }

        public void IncrementTurn()
        {
            _turnNumber++;
        }
    }
}
