using System;

namespace Chess.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public class TestNeededAttribute : Attribute
    {
        public TestNeededAttribute() { }
    }
}
