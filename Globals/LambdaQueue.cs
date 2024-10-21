using Chess.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Globals
{
    public static class LambdaQueue
    {
        private class LambdaAction
        {
            public Action Action { get; }
            public int Threshold { get; }
            public int CurrentCount { get; set; }

            public LambdaAction(Action action, int threshold)
            {
                Action = action;
                Threshold = threshold;
                CurrentCount = 0;
            }
        }
        private static List<LambdaAction> _actions = new();
        private const int DrainThreshold = 2;


        [TestNeeded]
        public static void Enqueue(Action action)
        {
            StaticLogger.Trace();
            StaticLogger.Log("Enqueuing action", LogLevel.Debug);
            if (action == null) throw new ArgumentNullException(nameof(action));

            lock (_actions)
            {
                _actions.Add(new LambdaAction(action, DrainThreshold));
            }
        }

        [TestNeeded]
        // Drain method to increment counters and invoke actions when ready
        public static void Drain()
        {
            StaticLogger.Trace();
            StaticLogger.Log("Drain invoked", LogLevel.Debug);
            var toRemove = new List<LambdaAction>();

            foreach (var lambdaAction in _actions)
            {
                lambdaAction.CurrentCount++;

                if (lambdaAction.CurrentCount >= lambdaAction.Threshold)
                {
                    StaticLogger.Log("An action has reached the threshold count - invoking and removing from queue", LogLevel.Debug);
                    lambdaAction.Action.Invoke();
                    toRemove.Add(lambdaAction); // Mark for removal after invocation
                }
            }

            // Remove the invoked actions from the list
            foreach (var action in toRemove)
            {
                // needs to be thread safe since tests are running in parallel and this is a global state being modified
                lock(_actions)
                {
                    StaticLogger.Log("Removing action", LogLevel.Debug);
                    _actions.Remove(action);
                }
            }
        }
    }
}
