using Chess.Attributes;
using System.Reflection;

namespace Tests
{
    public class TestAnalyzer : TestBase
    {
        private static List<string> FindMethodsNeedingTests()
        {
            List<string> strings = new();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    try
                    {
                        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                                          .Where(m => m.GetCustomAttribute(typeof(TestNeededAttribute), false) is not null)
                                          .ToList();
                        foreach (var method in methods)
                        {
                            strings.Add($"Method {method.Name} needs a unit test in Class {method.DeclaringType?.FullName}.");
                        }

                    }
                    catch (TypeLoadException e)
                    {
                        Console.WriteLine("TypeLoadException encountered. Ignoring");
                    }
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