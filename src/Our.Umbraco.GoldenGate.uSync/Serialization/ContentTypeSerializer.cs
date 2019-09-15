using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Our.Umbraco.GoldenGate.uSync.Mappers;
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
        private readonly MapperFactory _mapperFactory;

        public ContentTypeSerializer(IEntityService entityService, ILogger logger, IDataTypeService dataTypeService, IContentTypeService contentTypeService, IFileService fileService, MapperFactory mapperFactory)
            : base(entityService, logger, dataTypeService, contentTypeService, fileService)
        {
            _mapperFactory = mapperFactory;
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
            var infoNode = GetInfoNode(node);

            var key = GetKey(infoNode);
            var alias = GetAlias(infoNode);

            var contentType = new XElement("ContentType", node.Attributes(), node.Elements());

            contentType.Add(new XAttribute("Key", key));
            contentType.Add(new XAttribute("Alias", alias));

            var propertiesNode = GetPropertiesNode(contentType);

            if (propertiesNode != null)
            {
                var properties = propertiesNode.Elements("GenericProperty");

                MapProperties(properties);
            }

            return base.DeserializeCore(contentType);
        }

        private void MapProperties(IEnumerable<XElement> properties)
        {
            foreach (var property in properties)
            {
                var propertyTypeAlias = GetPropertyType(property);

                var mapper = _mapperFactory.GetPropertyTypeMapper(propertyTypeAlias);

                if (mapper != null)
                {
                    propertyTypeAlias = mapper.ConvertAlias(propertyTypeAlias);
                }

                property.Element("Type").SetValue(propertyTypeAlias);
            }
        }

        private XElement GetInfoNode(XElement node)
        {
            return node.Element("Info");
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
    }
}