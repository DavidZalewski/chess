namespace Chess.Services
{
    [Serializable]
    internal class ActionSequence
    {
        public Stack<Action> Actions { get; set; } = new Stack<Action>();
        public ActionSequence() { }

        public void AddActionInSequence(Action a)
        {
            Actions.Push(a);
        }

        public void PlayActionSequence()
        {
            foreach (Action a in Actions)
            {
                a.Invoke();
            }
        }
    }
}
