using Chess.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace Tests
{
    public class TestAnalyzer
    {
        public static void FindMethodsNeedingTests(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                              .Where(m => m.GetCustomAttributes(typeof(TestNeededAttribute), false).Length > 0)
                              .ToList();

            foreach (var method in methods)
            {
                Console.WriteLine($"Method {method.Name} needs a unit test in Class {method.DeclaringType?.FullName}.");
            }
        }
    }
}