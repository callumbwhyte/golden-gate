using System;
using System.Xml.Linq;
using Our.Umbraco.GoldenGate.uSync.Helpers;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using uSync8.Core;
using uSync8.Core.Extensions;
using uSync8.Core.Models;
using uSync8.Core.Serialization;
using Serializers = uSync8.Core.Serialization.Serializers;

namespace Our.Umbraco.GoldenGate.uSync.Serialization
{
    [SyncSerializer("B3073706-5037-4FBD-A015-DF38D61F2934", "MediaTypeSerializer", uSyncConstants.Serialization.MediaType)]
    public class MediaTypeSerializer : Serializers.MediaTypeSerializer, ISyncSerializer<IMediaType>
    {
        public MediaTypeSerializer(IEntityService entityService, ILogger logger, IDataTypeService dataTypeService, IMediaTypeService mediaTypeService)
            : base(entityService, logger, dataTypeService, mediaTypeService)
        {

        }

        public override bool IsValid(XElement node)
        {
            var infoNode = GetInfoNode(node);

            if (infoNode != null)
            {
                var key = GetKey(infoNode);
                var alias = GetAlias(infoNode);

                if (node.Name.LocalName == this.ItemType
                    && key != Guid.Empty
                    && alias != string.Empty)
                {
                    return true;
                }
            }

            return base.IsValid(node);
        }

        protected override SyncAttempt<IMediaType> DeserializeCore(XElement node)
        {
            var contentType = CreateContentType(node);

            return base.DeserializeCore(contentType);
        }

        public override SyncAttempt<IMediaType> DeserializeSecondPass(IMediaType item, XElement node, SerializerFlags flags)
        {
            var contentType = CreateContentType(node);

            return base.DeserializeSecondPass(item, contentType, flags);
        }

        private XElement CreateContentType(XElement node)
        {
            var infoNode = GetInfoNode(node);

            var keyAttr = GetKeyAttribute(node);
            if (keyAttr.Equals(Guid.Empty))
            {
                var key = GetKey(infoNode);
                var alias = GetAlias(infoNode);

                node.Add(new XAttribute("Key", key));
                node.Add(new XAttribute("Alias", alias));
            }

            var hasVortoProperties = false;
            var propertiesNode = GetPropertiesNode(node);
            if (propertiesNode != null)
            {
                var properties = propertiesNode.Elements("GenericProperty");

                foreach (var property in properties)
                {
                    var propertyType = GetPropertyType(property);

                    if (propertyType != string.Empty)
                    {
                        propertyType = PropertyTypeHelper.GetUpdatedAlias(propertyType);

                        if (VortoHelper.IsVortoType(propertyType))
                        {
                            var propertyDefinition = GetPropertyDefinition(property);
                            var vortoDataType = VortoHelper.GetDataType(propertyDefinition);
                            if (vortoDataType != null)
                            {
                                propertyType = vortoDataType.PropertyEditorAlias;
                                property.Element("Definition").SetValue(vortoDataType.Guid);

                                hasVortoProperties = true;
                                property.Add(new XElement("Variations", "Culture"));
                            }
                        }
                    }

                    property.Element("Type").SetValue(propertyType);
                }
            }

            if (hasVortoProperties)
            {
                infoNode.Add(new XElement("Variations", "Culture"));
            }

            //Rename Structure/DocumentType to Structure/ContentType
            var structureNode = GetStructureNode(node);
            if (structureNode != null)
            {
                foreach (var docTypeNode in structureNode.Elements("DocumentType"))
                {
                    docTypeNode.Name = "ContentType";
                }
            }            

            return node;
        }

        private XElement GetInfoNode(XElement node)
        {
            return node.Element("Info");
        }

        private XElement GetStructureNode(XElement node)
        {
            return node.Element("Structure");
        }

        private XElement GetPropertiesNode(XElement node)
        {
            return node.Element("GenericProperties");
        }

        private Guid GetKey(XElement node)
        {
            return node.Element("Key").ValueOrDefault(Guid.Empty);
        }

        private Guid GetKeyAttribute(XElement node)
        {
            return node.Attribute("Key").ValueOrDefault(Guid.Empty);
        }

        private string GetAlias(XElement node)
        {
            return node.Element("Alias").ValueOrDefault(string.Empty);
        }

        private string GetPropertyType(XElement node)
        {
            return node.Element("Type").ValueOrDefault(string.Empty);
        }

        private Guid GetPropertyDefinition(XElement node)
        {
            return node.Element("Definition").ValueOrDefault(Guid.Empty);
        }
    }
}