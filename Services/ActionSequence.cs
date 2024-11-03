using Chess.Attributes;
using Chess.Globals;

namespace Chess.Services
{
    [Serializable]
    internal class ActionSequence
    {
        public Stack<Action> Actions { get; set; } = new Stack<Action>();
        public ActionSequence() { }

        public void AddActionInSequence(Action a)
        {
            StaticLogger.Trace();
            Actions.Push(a);
        }

        public void PlayActionSequence()
        {
            StaticLogger.Trace();
            foreach (Action a in Actions)
            {
                a.Invoke();
            }
        }

        public bool IsActionInSequence(Action actionToFind)
        {
            StaticLogger.Trace();
            return Actions.Any(a => a.Equals(actionToFind));
        }
    }
}
