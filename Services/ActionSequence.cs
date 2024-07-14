using Chess.Attributes;

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
            Actions.Push(a);
        }

        [TestNeeded]
        public void PlayActionSequence()
        {
            foreach (Action a in Actions)
            {
                a.Invoke();
            }
        }

        [TestNeeded]
        public bool IsActionInSequence(Action actionToFind)
        {
            return Actions.Any(a => a.Equals(actionToFind));
        }
    }
}
