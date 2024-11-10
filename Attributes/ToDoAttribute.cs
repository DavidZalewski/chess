using System;
using System.Diagnostics;
using System.Reflection;

namespace Chess.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public class ToDoAttribute : Attribute
    {
        private readonly string value;
        private static List<string> values = new List<string>();
        public ToDoAttribute(string v) {
            value = v;
        }

        internal static void Add(string v)
        {
            MethodBase? method = GetStackTraceInfo();
            values.Add($"source: {method?.Name}, value: {v}");
        }

        public string GetValue() { return value; }

        public static string? GetValues()
        {
            string? output = values?.Select((v) => v + "\n")?.ToString();
            return output;
        }

        private static MethodBase? GetStackTraceInfo()
        {
            // Retrieve the stack trace to get the caller information
            var stackTrace = new StackTrace(2, true); // Skip the first two frames (this method, and the one above it)
            var frame = stackTrace.GetFrame(0); // Get the calling method's frame
            return frame?.GetMethod();
        }

    }
}
