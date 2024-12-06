using Chess.Board;
using Chess.Pieces;

namespace Chess.GameState
{
    public class ChessAnalysisResult
    {
        // List of all pieces currently attacking, with their targets
        public List<AttackInfo> Attacks { get; set; } = new List<AttackInfo>();

        // Information about forks
        public List<ForkInfo> Forks { get; set; } = new List<ForkInfo>();

        // Information about pinned pieces
        public List<PinInfo> Pins { get; set; } = new List<PinInfo>();

        // Information about covered pieces
        public List<CoverageInfo> Coverages { get; set; } = new List<CoverageInfo>();

        // Possible future moves (can be simplified or expanded based on depth or strategy)
        public List<MoveInfo> PossibleMoves { get; set; } = new List<MoveInfo>();

        // Predicted opponent moves or strategy (placeholder for now)
        public List<MoveInfo> PredictedOpponentMoves { get; set; } = new List<MoveInfo>();
    }

    public class AttackInfo
    {
        public ChessPiece Attacker { get; set; }
        public List<ChessPiece> Targets { get; set; } = new List<ChessPiece>();
    }

    public class ForkInfo
    {
        public ChessPiece ForkingPiece { get; set; }
        public List<ChessPiece> ForkedPieces { get; set; } = new List<ChessPiece>();
    }

    public class PinInfo
    {
        public ChessPiece PinnedPiece { get; set; }
        public ChessPiece PinningPiece { get; set; }
    }

    public class CoverageInfo
    {
        public ChessPiece CoveredPiece { get; set; }
        public List<ChessPiece> CoveringPieces { get; set; } = new List<ChessPiece>();
    }

    public class MoveInfo
    {
        public ChessPiece MovingPiece { get; set; }
        public BoardPosition From { get; set; }
        public BoardPosition To { get; set; }
        public bool IsCheck { get; set; }
        public bool IsCapture { get; set; }
    }
}
