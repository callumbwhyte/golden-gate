using System.Xml.Linq;
using Newtonsoft.Json;
using Umbraco.Core;

namespace Our.Umbraco.GoldenGate.uSync.Convertors
{
    public class MultiNodeTreePickerConvertor : DataTypeConvertor
    {
        public override string GetPropertyAlias()
        {
            return Constants.PropertyEditors.Aliases.MultiNodeTreePicker;
        }

        protected override object ProcessPrevalue(XElement node, string preValueAlias, dynamic config)
        {
            switch (preValueAlias)
            {
                case "filter":
                    config.Filter = GetNodeValue(node);
                    break;

                case "maxNumber":
                    if (int.TryParse(GetNodeValue(node), out var max))
                    {
                        config.MaxNumber = max;
                    }
                    break;

                case "minNumber":
                    if (int.TryParse(GetNodeValue(node), out var min))
                    {
                        config.MinNumber = min;
                    }
                    break;

                case "showOpenButton":
                    config.ShowOpen = "1".Equals(GetNodeValue(node));
                    break;

                case "startNode":
                    config.TreeSource = JsonConvert.DeserializeObject(GetNodeValue(node));
                    break;

                default:
                    throw new System.Exception($"Unknown PreValue alias '{preValueAlias}'");

            }

            return null;
        }
    }
}