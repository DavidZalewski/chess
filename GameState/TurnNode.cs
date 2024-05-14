using Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameState
{
    internal class TurnNode
    {
        public string BoardState { get; set; }
        public string TurnDescription { get; set; }
        public int TurnNumber { get; set; }
        public TurnNode? Parent { get; set; }
        public List<TurnNode> Children { get; set; } = new List<TurnNode>();
        public string Command { get; set; }

        public TurnNode(Turn turn, string optionalText = "")
        {
            TurnDescription = turn.TurnDescription + optionalText;
            TurnNumber = turn.TurnNumber;
            BoardState = turn.ChessBoard.DisplayBoard();
            //Command = turn.Command;

            if (String.IsNullOrEmpty(Command))
            {
                if (turn.ChessPiece is ChessPiecePawn)
                {
                    Command = turn.PlayerTurn.Equals(Turn.Color.WHITE) ? "W" : "B" + "P" + turn.ChessPiece.GetId() + " " + turn.NewPosition.StringValue;
                }
                else if (turn.ChessPiece is ChessPieceKnight)
                {
                    Command = turn.PlayerTurn.Equals(Turn.Color.WHITE) ? "W" : "B" + "K" + turn.ChessPiece.GetId() + " " + turn.NewPosition.StringValue;
                }
                else if (turn.ChessPiece is ChessPieceBishop)
                {
                    Command = turn.PlayerTurn.Equals(Turn.Color.WHITE) ? "W" : "B" + "B" + turn.ChessPiece.GetId() + " " + turn.NewPosition.StringValue;
                }
                else if (turn.ChessPiece is ChessPieceRook)
                {
                    Command = turn.PlayerTurn.Equals(Turn.Color.WHITE) ? "W" : "B" + "R" + turn.ChessPiece.GetId() + " " + turn.NewPosition.StringValue;
                }
                else if (turn.ChessPiece is ChessPieceQueen)
                {
                    Command = turn.PlayerTurn.Equals(Turn.Color.WHITE) ? "W" : "B" + "Q" + turn.ChessPiece.GetId() + " " + turn.NewPosition.StringValue;
                }
                else if (turn.ChessPiece is ChessPieceKing)
                {
                    Command = turn.PlayerTurn.Equals(Turn.Color.WHITE) ? "W" : "B" + "K " + turn.NewPosition.StringValue;
                }
            }
        }
    }
}
