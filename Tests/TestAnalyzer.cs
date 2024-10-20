using Chess.Attributes;
using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Reflection;

namespace Tests
{
    public class TestAnalyzer
    {
        public static List<string> FindMethodsNeedingTests()
        {
            List<string> strings = new();
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                // TODO: Figure out why this isn't returning anything
                // It should return at least 6 different cases of this attribute marking a method as needing a test
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                  .Where(m => m.GetCustomAttribute(typeof(TestNeededAttribute), false) is not null)
                  .ToList();
                if (methods.Count > 0)
                {
                    Console.WriteLine("Breakpoint");
                }
                foreach (var method in methods)
                {
                    strings.Add($"Method {method.Name} needs a unit test in Class {method.DeclaringType?.FullName}.");
                }
            }

            return strings;
        }

        [Test]
        public void MethodsNeedingTestShouldBeLessThan3() 
        {
            List<string> strings = FindMethodsNeedingTests();
            foreach(string method in strings)
            {
                Console.WriteLine(method);
            }
            Assert.That(strings.Count, Is.LessThan(3));
        }
    }
}