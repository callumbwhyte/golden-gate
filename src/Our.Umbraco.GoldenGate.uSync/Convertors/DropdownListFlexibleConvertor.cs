using System.Xml.Linq;
using Our.Umbraco.GoldenGate.uSync.Models;
using Umbraco.Core;
using uSync8.Core.Extensions;

namespace Our.Umbraco.GoldenGate.uSync.Convertors
{
    public class DropdownListFlexibleConvertor : DataTypeConvertor
    {
        public override string GetPropertyAlias()
        {
            return Constants.PropertyEditors.Aliases.DropDownListFlexible;
        }

        protected override object ProcessPrevalue(XElement node, string preValueAlias, dynamic config)
        {
            switch (preValueAlias)
            {
                case "multiple":
                    config.Multiple = "1".Equals(GetNodeValue(node));
                    break;

                default:
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

            return null;
        }
    }
}