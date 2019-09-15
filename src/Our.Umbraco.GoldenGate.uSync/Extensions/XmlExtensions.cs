using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using uSync8.Core.Extensions;

namespace Our.Umbraco.GoldenGate.uSync.Extensions
{
    public static class XmlExtensions
    {
        public static string GetValue(this XElement element, string name)
        {
            return element.ValueOrDefault(string.Empty);
        }

        public static T GetValue<T>(this XElement element, string name)
        {
            return element.ValueOrDefault(default(T));
        }

        public static XElement GetElement(this XElement element, string name)
        {
            return element.Element(name);
        }

        public static IEnumerable<XElement> GetElements(this XElement element, string name)
        {
            return element.Elements(name).Where(x => x != null);
        }

        public static string GetAttribute(this XElement element, string name)
        {
            return element.Attribute(name).ValueOrDefault(string.Empty);
        }

        public static T GetAttribute<T>(this XElement element, string name)
            where T : struct
        {
            return element.Attribute(name).ValueOrDefault(default(T));
        }

        public static void AddElement(this XElement element, string name, object value)
        {
            element.Add(new XElement(name, value));
        }

        public static void AddAttribute(this XElement element, string name, object value)
        {
            element.Add(new XAttribute(name, value));
        }
    }
}