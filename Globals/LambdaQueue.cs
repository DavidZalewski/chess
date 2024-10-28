using Chess.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Globals
{
    public static class LambdaQueue
    {
        // New LambdaAction delegate that accepts a GameController
        private class LambdaAction
        {
            public Action<Chess.Controller.GameController> Action { get; }
            public int Threshold { get; }
            public int CurrentCount { get; set; }

            public LambdaAction(Action<Chess.Controller.GameController> action, int threshold)
            {
                Action = action;
                Threshold = threshold;
                CurrentCount = 0;
            }
        }

        private static List<LambdaAction> _actions = new();
        private const int DrainThreshold = 2;

        // Modified Enqueue method to accept Action with GameController parameter
        public static void Enqueue(Action<Chess.Controller.GameController> action)
        {
            StaticLogger.Trace();
            StaticLogger.Log("Enqueuing action", LogLevel.Debug);
            if (action == null) throw new ArgumentNullException(nameof(action));

            lock (_actions)
            {
                _actions.Add(new LambdaAction(action, DrainThreshold));
            }
        }

        // Modified Drain method to pass the GameController when invoking the lambda
        public static void Drain(Chess.Controller.GameController gc)
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
                    lambdaAction.Action.Invoke(gc);  // Pass the GameController here
                    toRemove.Add(lambdaAction); // Mark for removal after invocation
                }
            }

            // Remove the invoked actions from the list
            foreach (var action in toRemove)
            {
                lock (_actions)
                {
                    StaticLogger.Log("Removing action", LogLevel.Debug);
                    _actions.Remove(action);
                }
            }
        }
    }
}
