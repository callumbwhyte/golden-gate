using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Our.Umbraco.GoldenGate.uSync.Convertors;
using Our.Umbraco.GoldenGate.uSync.Models;
using uSync8.Core.Extensions;

namespace Our.Umbraco.GoldenGate.uSync.Helpers
{
    public class VortoHelper
    {
        private const string PropertyAlias = "Our.Umbraco.Vorto";

        private static Dictionary<Guid, VortoDataType> DataTypes = new Dictionary<Guid, VortoDataType>();

        public static bool IsVortoType(string alias)
        {
            return PropertyAlias.Equals(alias);
        }

        public static bool IsVortoType(XElement node, string alias)
        {
            if (PropertyAlias.Equals(alias))
            {
                //Get underlying datatype
                var preValues = node.Element("PreValues");
                if (preValues != null)
                {
                    var dataTypeNode = preValues.Elements("PreValue")
                        .Where(pv => "dataType".Equals(pv.Attribute("Alias").ValueOrDefault(string.Empty)))
                        .FirstOrDefault();

                    var dataType = JsonConvert.DeserializeObject<VortoDataType>(DataTypeConvertor.GetNodeValue(dataTypeNode));
                    if (dataType != null)
                    {
                        var key = GetKey(node);
                        if (!DataTypes.ContainsKey(key))
                        {
                            dataType.PropertyEditorAlias = PropertyTypeHelper.GetUpdatedAlias(dataType.PropertyEditorAlias);

                            DataTypes.Add(key, dataType);
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        public static VortoDataType GetDataType(Guid guid)
        {
            if (DataTypes.TryGetValue(guid, out var dataType))
            {
                return dataType;
            }

            return null;
        }

        private static Guid GetKey(XElement node)
        {
            return node.Attribute("Key").ValueOrDefault(Guid.Empty);
        }
    }
}