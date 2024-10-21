using Chess.Attributes;
using Chess.Globals;

namespace Chess.Services
{
    [Serializable]
    internal class ActionSequence
    {
        public Stack<Action> Actions { get; set; } = new Stack<Action>();
        public ActionSequence() { }

        [TestNeeded]
        public void AddActionInSequence(Action a)
        {
            StaticLogger.Trace();
            Actions.Push(a);
        }

        [TestNeeded]
        public void PlayActionSequence()
        {
            StaticLogger.Trace();
            foreach (Action a in Actions)
            {
                a.Invoke();
            }
        }

        [TestNeeded]
        public bool IsActionInSequence(Action actionToFind)
        {
            StaticLogger.Trace();
            return Actions.Any(a => a.Equals(actionToFind));
        }
    }
}
