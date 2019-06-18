using System.Xml.Linq;
using Our.Umbraco.GoldenGate.uSync.Models;
using Umbraco.Core;
using uSync8.Core.Extensions;

namespace Our.Umbraco.GoldenGate.uSync.Convertors
{
    public class CheckBoxListConvertor : DataTypeConvertor
    {
        public override string GetPropertyAlias()
        {
            return Constants.PropertyEditors.Aliases.CheckBoxList;
        }

        protected override object ProcessPrevalue(XElement node, string preValueAlias, dynamic config)
        {
            if (int.TryParse(preValueAlias, out var a))
            {
                return new PreValue
                {
                    Id = node.Attribute("SortOrder").ValueOrDefault(0),
                    Value = GetNodeValue(node)
                };
            }
            else
            {
                throw new System.Exception($"Unknown PreValue alias '{preValueAlias}'");
            }
        }
    }
}