using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Board;

namespace Chess.Pieces
{
    [Serializable]
    public class ChessPieceWhitePawn : ChessPiecePawn
    {
        public ChessPieceWhitePawn(int id, BoardPosition startingPosition) : base(Color.WHITE, id, startingPosition)
        {
            _realValue = 11; // could also calculate this in base class by adding the two enums together
        }

        public override ChessPiece Clone()
        {
            ChessPieceWhitePawn copy = new(_id, _startingPosition);
            return Clone(copy);
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            // get the distance
            //   2       =                    6                        4
            int verticalDistance = _currentPosition.VerticalValueAsInt - position.VerticalValueAsInt;

            // TODO: embessen move check
            // TODO: handle promotions

            if (verticalDistance == 1)
            {
                if (_currentPosition.HorizontalValue == position.HorizontalValue)
                {
                    // is there a piece on this position that is blocking the pawn from moving?
                    if (board.IsPieceAtPosition(position))
                        return false;
                    else
                        return true;
                }
                // if the position to move pawn is on a different horizontal value, check if its a valid capture
                else
                {
                    int horizontalDistance = _currentPosition.HorizontalValueAsInt - position.HorizontalValueAsInt;
                    // adjacent and there is a black piece there?
                    if (board.IsPieceAtPosition(position, Color.BLACK) && (horizontalDistance == 1 || horizontalDistance == -1))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            // if the pawn hasn't moved yet, it can jump 2 squares instead of 1
            else if (verticalDistance == 2 && _currentPosition.Equals(_startingPosition))
            {
                if (_currentPosition.HorizontalValue == position.HorizontalValue)
                {
                    BoardPosition previousSquare = new BoardPosition((BoardPosition.VERTICAL)position.VerticalValueAsInt + 1, position.HorizontalValue);
                    // is there a piece in front of it that it is trying to jump over?
                    if (board.IsPieceAtPosition(previousSquare) || board.IsPieceAtPosition(position))
                        return false;
                    else
                        return true;
                }      
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        protected override bool ImplementMove(ChessBoard board, BoardPosition position)
        {
            // does this need to exist?
            return false;
        }

        public override IEnumerable<BoardPosition> GetValidMoves(ChessBoard board)
        {
            // ... Your existing move generation logic ...
            // At this point in the code there is nothing that is generating possible moves, as we first have to check which moves are valid
            // This response avoids answering the actual question.
            BoardPosition position;

            // En Passant Move Check
            var lastMove = GameController?.GetLastTurn();
            if (lastMove != null && lastMove.ChessPiece is ChessPiecePawn &&
                Math.Abs(lastMove.NewPosition.VerticalValueAsInt - lastMove.PreviousPosition.VerticalValueAsInt) == 2 && // moved two squares
                lastMove.PreviousPosition.HorizontalValueAsInt == _currentPosition.HorizontalValueAsInt && // landed beside this pawn
                /* Unclear: what is position here? Which position for which piece? */ 
                position.VerticalValueAsInt == lastMove.NewPosition.VerticalValueAsInt - 1 && // target square is behind
                Math.Abs(position.HorizontalValueAsInt - _currentPosition.HorizontalValueAsInt) == 1)
            {
                yield return position;
            }
        }

    }
}
