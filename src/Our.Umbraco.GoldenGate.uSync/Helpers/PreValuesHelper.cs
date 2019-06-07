using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Our.Umbraco.GoldenGate.uSync.Models;
using Umbraco.Core;
using uSync8.Core.Extensions;

namespace Our.Umbraco.GoldenGate.uSync.Helpers
{
    public static class PreValuesHelper
    {
        public static PreValues GetPrevalues(XElement node, string alias)
        {
            //Only processing these datatypes for now, to avoid surprises
            if (alias == Constants.PropertyEditors.Aliases.CheckBoxList
                || alias == Constants.PropertyEditors.Aliases.MultiNodeTreePicker
                || alias == Constants.PropertyEditors.Aliases.Grid
                || alias == Constants.PropertyEditors.Aliases.DropDownListFlexible)
            {
                var prevaluesNode = node.Element("PreValues");
                if (prevaluesNode != null)
                {
                    var result = new PreValues();
                    var valueList = new List<PreValue>();

                    var values = prevaluesNode.Elements("PreValue").ToList();
                    foreach (var v in values)
                    {
                        var preValueAlias = v.Attribute("Alias").ValueOrDefault(string.Empty);
                        switch (preValueAlias)
                        {
                            case "filter":
                                result.Filter = GetNodeValue(v);
                                break;
                            case "maxNumber":
                                if (int.TryParse(GetNodeValue(v), out var max))
                                {
                                    result.MaxNumber = max;
                                }
                                break;
                            case "minNumber":
                                if (int.TryParse(GetNodeValue(v), out var min))
                                {
                                    result.MinNumber = min;
                                }
                                break;
                            case "showOpenButton":
                                result.ShowOpen = "1".Equals(GetNodeValue(v));
                                break;
                            case "multiple":
                                result.Multiple = "1".Equals(GetNodeValue(v));
                                break;
                            case "startNode":
                                result.TreeSource = JsonConvert.DeserializeObject(GetNodeValue(v));
                                break;
                            case "items":
                                result.Items = JsonConvert.DeserializeObject(GetNodeValue(v));
                                break;
                            case "rte":
                                result.Rte = JsonConvert.DeserializeObject(GetNodeValue(v));

                                var toolbar = result.Rte.toolbar as JArray;
                                if (toolbar != null)
                                {
                                    var match = toolbar.FirstOrDefault(j => j.ToString().Equals("code"));
                                    if (match != null)
                                    {
                                        toolbar.Remove(match);
                                        toolbar.Insert(0, new JValue("ace"));
                                    }
                                }

                                break;
                            default:
                                if (int.TryParse(preValueAlias, out var a))
                                {
                                    valueList.Add(new PreValue
                                    {
                                        Id = v.Attribute("SortOrder").ValueOrDefault(0),
                                        Value = GetNodeValue(v)
                                    });
                                }
                                else
                                {
                                    throw new System.Exception($"Unknown PreValue alias '{preValueAlias}'");
                                }
                                break;
                        }
                    }

                    if (valueList.Any())
                    {
                        result.Items = valueList;
                    }
                    return result;
                }
            }

            return null;
        }

        private static string GetNodeValue(XElement node)
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