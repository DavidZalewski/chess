using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ChessPieceWhitePawn : ChessPiecePawn
    {
        public ChessPieceWhitePawn(int id, BoardPosition startingPosition) : base(Color.WHITE, id, startingPosition)
        {
            _realValue = 11; // could also calculate this in base class by adding the two enums together
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
            else if (verticalDistance == 2 && (_currentPosition.Equals(_startingPosition)))
            {
                if (_currentPosition.HorizontalValue == position.HorizontalValue)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        protected override void ImplementMove(ChessBoard board, BoardPosition position)
        {
            // does this need to exist?
            
        }
    }
}
