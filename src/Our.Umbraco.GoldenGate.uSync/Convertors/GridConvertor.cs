using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Core;

namespace Our.Umbraco.GoldenGate.uSync.Convertors
{
    public class GridConvertor : DataTypeConvertor
    {
        public override string GetPropertyAlias()
        {
            return Constants.PropertyEditors.Aliases.Grid;
        }

        protected override object ProcessPrevalue(XElement node, string preValueAlias, dynamic config)
        {
            switch (preValueAlias)
            {
                case "items":
                    config.Items = JsonConvert.DeserializeObject(GetNodeValue(node));
                    break;

                case "rte":
                    dynamic rte = JsonConvert.DeserializeObject(GetNodeValue(node));

                    var toolbar = rte.toolbar as JArray;
                    if (toolbar != null)
                    {
                        var match = toolbar.FirstOrDefault(j => j.ToString().Equals("code"));
                        if (match != null)
                        {
                            toolbar.Remove(match);
                            toolbar.Insert(0, new JValue("ace"));
                        }
                    }

                    config.Rte = rte;

                    break;

                default:
                    throw new System.Exception($"Unknown PreValue alias '{preValueAlias}'");

            }

            return null;
        }
    }
}