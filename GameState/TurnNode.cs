using Chess.Pieces;

namespace Chess.GameState
{
    internal class TurnNode
    {
        public string BoardState { get; set; }
        public string TurnDescription { get; set; }
        public int TurnNumber { get; set; }
        public TurnNode? Parent { get; set; }
        public List<TurnNode> Children { get; set; } = new List<TurnNode>();
        public ulong Count { get; set; }
        public string Command { get; set; }
        public string TurnID { get; }
        public string BoardID { get; }

        public TurnNode(Turn turn, string optionalText = "")
        {
            TurnDescription = turn.TurnDescription + optionalText;
            TurnNumber = turn.TurnNumber;
            BoardState = "PLACEHOLDER"; // turn.ChessBoard.DisplayBoard();
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

            TurnID = TurnNumber + ":" 
                    + (turn.PlayerTurn.Equals(Turn.Color.WHITE) ? "W" : "B") + ":" 
                    + turn.ChessPiece.GetPieceName() + ":"
                    + turn.Command + ":FROM:" 
                    + turn.PreviousPosition.StringValue;

            BoardID = turn.ChessBoard.BoardID;
        }
    }
}
