using Chess.Attributes;
using Chess.Globals;

namespace Chess.Exceptions
{
    [ToDo("Use this or remove it. No need for unused classes.")]
    internal class InvalidMoveException : Exception
    {
        public InvalidMoveException() {
            StaticLogger.Trace();
        }
    }
}
