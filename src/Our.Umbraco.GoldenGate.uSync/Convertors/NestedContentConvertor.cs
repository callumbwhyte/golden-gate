using System.Xml.Linq;
using Newtonsoft.Json;
using Umbraco.Core;

namespace Our.Umbraco.GoldenGate.uSync.Convertors
{
    public class NestedContentConvertor : DataTypeConvertor
    {
        public override string GetPropertyAlias()
        {
            return Constants.PropertyEditors.Aliases.NestedContent;
        }

        protected override object ProcessPrevalue(XElement node, string preValueAlias, dynamic config)
        {
            switch (preValueAlias)
            {
                case "contentTypes":
                    config.ContentTypes = JsonConvert.DeserializeObject(GetNodeValue(node));
                    break;

                case "minItems":
                    if (int.TryParse(GetNodeValue(node), out var max))
                    {
                        config.MinItems = max;
                    }
                    break;

                case "maxItems":
                    if (int.TryParse(GetNodeValue(node), out var min))
                    {
                        config.MaxItems = min;
                    }
                    break;

                case "confirmDeletes":
                    config.ConfirmDeletes = "1".Equals(GetNodeValue(node));
                    break;

                case "showIcons":
                    config.ShowIcons = "1".Equals(GetNodeValue(node));
                    break;

                case "hideLabel":
                    config.HideLabel = "1".Equals(GetNodeValue(node));
                    break;

                default:
                    throw new System.Exception($"Unknown PreValue alias '{preValueAlias}'");

            }

            return null;
        }
    }
}