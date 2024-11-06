using Chess.Globals;
using Chess.Pieces;
using System;
using System.Reflection;

namespace Tests
{
    [TestFixture]
    [Category("CORE")]
    public class ReflectionTests
    {
        public int AddNumbers(int a, int b)
        {
            return a + b;
        }

        public void ComplexFunction(Dictionary<string, LogParameter> logParamDictionary, int count, string metaData, ChessPiece chessPiece)
        {
            
        }


        [Test]
        public void Test_Getting_Parameter_Names_Via_Reflection_Method_AddNumbers()
        {
            ReflectionTests test = new ReflectionTests();
            MethodInfo? method = typeof(ReflectionTests).GetMethod("AddNumbers");

            Assert.That(method, Is.Not.Null);

            ParameterInfo[] parameters = method.GetParameters();

            Assert.That(parameters.Length, Is.EqualTo(2));
            Assert.That(parameters[0].Name, Is.EqualTo("a"));
            Assert.That(parameters[1].Name, Is.EqualTo("b"));
        }

        [Test]
        public void Test_Getting_Parameter_Names_Via_Reflection_Method_ComplexFunction()
        {
            ReflectionTests test = new ReflectionTests();
            MethodInfo? method = typeof(ReflectionTests).GetMethod("ComplexFunction");

            Assert.That(method, Is.Not.Null);

            ParameterInfo[] parameters = method.GetParameters();

            Assert.That(parameters.Length, Is.EqualTo(4));
            Assert.That(parameters[0].Name, Is.EqualTo("logParamDictionary"));
            Assert.That(parameters[1].Name, Is.EqualTo("count"));
            Assert.That(parameters[2].Name, Is.EqualTo("metaData"));
            Assert.That(parameters[3].Name, Is.EqualTo("chessPiece"));
        }

    }
}
