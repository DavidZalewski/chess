using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Globals
{
    public static class ToStringTrait
    {
        public static string ToDetailedString(this object obj)
        {
            if (obj == null)
            {
                return "Object is null";
            }

            var type = obj.GetType();
            var sb = new StringBuilder();

            // Do not dump .net collections properties - we dont care about the inner workings of a list or stack
            // we just need to see what the collection is holding data wise
            Type[] interfaces = type.GetInterfaces();
            foreach ( var i in interfaces )
            {
                if (i.Name.Contains(typeof(IEnumerable).Name, StringComparison.OrdinalIgnoreCase) ||
                    i.Name.Contains(typeof(ICollection).Name, StringComparison.OrdinalIgnoreCase) ||
                    i.Name.Contains(typeof(IList).Name, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var item in (IEnumerable)obj)
                    {
                        sb.Append(item.ToDetailedString());
                    }
                    return sb.ToString();

                }
            }

            // TODO: Figure out how to print out only primitive data types (int, double, string, char, enum) and my class types (Chess namespace) ONLY
            if (type.FullName.StartsWith("Chess") || type.IsPrimitive)
            {
                var properties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic
    | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy);
                sb.AppendLine($"Class: {type.Name}");
                foreach (var property in properties)
                {
                    try
                    {
                        object value = property.GetValue(obj, null);
                        sb.AppendLine($"    {property.Name} {value}");
                    }
                    catch (System.NotSupportedException e)
                    {
                        // just continue
                        Console.WriteLine($"ToStringTrait: Caught NotSupportedException for Property: {property.Name} - Message: {e.Message} - IGNORING");
                    }
                }
                return sb.ToString();
            }

            return "";
        }
    }
}
