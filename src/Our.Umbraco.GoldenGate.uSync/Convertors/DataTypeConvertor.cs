using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml.Linq;
using uSync8.Core.Extensions;

namespace Our.Umbraco.GoldenGate.uSync.Convertors
{
    public abstract class DataTypeConvertor
    {
        public abstract string GetPropertyAlias();

        protected abstract object ProcessPrevalue(XElement node, string preValueAlias, dynamic config);

        protected XElement GetPreValuesNode(XElement node)
        {
            return node.Element("PreValues");
        }

        public virtual dynamic GetConfig(XElement node, string alias)
        {
            if (alias.Equals(GetPropertyAlias()))
            {
                var prevaluesNode = GetPreValuesNode(node);
                if (prevaluesNode != null)
                {
                    dynamic config = new ExpandoObject();
                    var valueList = new List<object>();

                    var values = prevaluesNode.Elements("PreValue").ToList();
                    foreach (var v in values)
                    {
                        var preValueAlias = v.Attribute("Alias").ValueOrDefault(string.Empty);
                        var o = ProcessPrevalue(v, preValueAlias, config);
                        if (o != null)
                        {
                            valueList.Add(o);
                        }
                    }

                    if (valueList.Any())
                    {
                        config.Items = valueList;
                    }

                    return config;
                }
            }

            return null;
        }

        public static string GetNodeValue(XElement node)
        {
            var nodeValue = node.FirstNode;
            if (nodeValue is XCData)
            {
                var cdata = ((XCData)nodeValue).Value;
                return string.IsNullOrEmpty(cdata) ? null : cdata;
            }
            else
            {
                return string.IsNullOrEmpty(node.Value) ? null : node.Value;
            }
        }
    }
}