using Chess.Attributes;
using System.Reflection;

namespace Tests.Setup
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

        private static List<string> FindToDos()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            List<string> strings = new();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    try
                    {
                        var toDosOnClass = type.GetCustomAttributes(typeof(ToDoAttribute), true);

                        foreach (object o in toDosOnClass)
                        {
                            Assert.That(o is ToDoAttribute);
                            strings.Add($"Class {type.FullName} has ToDo [{((ToDoAttribute)o).GetValue()}]");
                        }

                        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                                          .Where(m => m.GetCustomAttribute(typeof(ToDoAttribute), false) is not null)
                                          .ToList();
                        foreach (var method in methods)
                        {
                            string key = $"{method.DeclaringType?.FullName}.{method.Name}";
                            if (!dic.ContainsKey(key))
                            {
                                strings.Add($"Method {method.DeclaringType?.FullName}.{method.Name} has ToDo [{((ToDoAttribute)method.GetCustomAttribute(typeof(ToDoAttribute))).GetValue()}]");
                                dic.Add(key, 1);
                            }
                        }

                    }
                    catch (TypeLoadException e)
                    {
                        Console.WriteLine("TypeLoadException encountered. Ignoring");
                    }
                }
            }

            strings.Add(ToDoAttribute.GetValues());

            return strings;
        }


        [Test]
        public void MethodsNeedingTestShouldBeLessThan3()
        {
            List<string> strings = FindMethodsNeedingTests();
            foreach (string method in strings)
            {
                Console.WriteLine(method);
            }
            Assert.That(strings.Count, Is.LessThan(3));
        }

        [Test]
        public void ToDoListIsLessThan3()
        {
            List<string> strings = FindToDos();
            foreach (string entry in strings)
            {
                Console.WriteLine(entry);
            }
            Assert.That(strings.Count, Is.LessThan(3));
        }

    }
}