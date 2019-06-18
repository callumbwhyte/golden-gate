using System;
using System.Collections.Generic;
using System.Linq;
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
    [SyncSerializer("B3F7F247-6077-406D-8480-DB1004C8211C", "ContentTypeSerializer", uSyncConstants.Serialization.ContentType)]
    public class ContentTypeSerializer : Serializers.ContentTypeSerializer, ISyncSerializer<IContentType>
    {
        public ContentTypeSerializer(IEntityService entityService, ILogger logger, IDataTypeService dataTypeService, IContentTypeService contentTypeService, IFileService fileService)
            : base(entityService, logger, dataTypeService, contentTypeService, fileService)
        {

        }

        public override bool IsValid(XElement node)
        {
            var infoNode = GetInfoNode(node);

            if (infoNode != null)
            {
                var key = GetKey(infoNode);
                var alias = GetAlias(infoNode);

                if (node.Name.LocalName == "DocumentType"
                    && key != Guid.Empty
                    && alias != string.Empty)
                {
                    return true;
                }
            }

            return base.IsValid(node);
        }

        protected override SyncAttempt<IContentType> DeserializeCore(XElement node)
        {
            var contentType = CreateContentType(node);

            return base.DeserializeCore(contentType);
        }

        public override SyncAttempt<IContentType> DeserializeSecondPass(IContentType item, XElement node, SerializerFlags flags)
        {
            var contentType = CreateContentType(node);

            return base.DeserializeSecondPass(item, contentType, flags);
        }

        private XElement CreateContentType(XElement node)
        {
            if (node.Name.LocalName == "DocumentType")
            {
                var infoNode = GetInfoNode(node);

                var key = GetKey(infoNode);
                var alias = GetAlias(infoNode);

                //Rename node
                node.Name = "ContentType";

                node.Add(new XAttribute("Key", key));
                node.Add(new XAttribute("Alias", alias));

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