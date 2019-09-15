using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Our.Umbraco.GoldenGate.uSync.Extensions;
using Our.Umbraco.GoldenGate.uSync.Mappers;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using uSync8.Core.Models;
using Serializers = uSync8.Core.Serialization.Serializers;

namespace Our.Umbraco.GoldenGate.uSync.Serialization
{
    public class ContentTypeSerializer : Serializers.ContentTypeSerializer
    {
        private readonly MapperFactory _mapperFactory;

        public ContentTypeSerializer(IEntityService entityService, ILogger logger, IDataTypeService dataTypeService, IContentTypeService contentTypeService, IFileService fileService, MapperFactory mapperFactory)
            : base(entityService, logger, dataTypeService, contentTypeService, fileService)
        {
            _mapperFactory = mapperFactory;
        }

        public override bool IsValid(XElement node)
        {
            var infoNode = node.GetElement("Info");

            if (infoNode != null)
            {
                var key = infoNode.GetValue<Guid>("Key");
                var alias = infoNode.GetValue("Alias");

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
            var infoNode = node.GetElement("Info");

            var key = infoNode.GetValue<Guid>("Key");
            var alias = infoNode.GetValue("Alias");

            var contentType = new XElement("ContentType", node.Attributes(), node.Elements());

            contentType.AddAttribute("Key", key);
            contentType.AddAttribute("Alias", alias);

            var propertiesNode = contentType.GetElement("GenericProperties");

            if (propertiesNode != null)
            {
                var properties = propertiesNode.GetElements("GenericProperty");

                if (properties.Any() == true)
                {
                    MapProperties(properties);
                }
            }

            return base.DeserializeCore(contentType);
        }

        private void MapProperties(IEnumerable<XElement> properties)
        {
            foreach (var property in properties)
            {
                var propertyTypeAlias = property.GetValue("Type");

                var mapper = _mapperFactory.GetPropertyTypeMapper(propertyTypeAlias);

                if (mapper != null)
                {
                    propertyTypeAlias = mapper.ConvertAlias(propertyTypeAlias);
                }

                property.GetElement("Type").SetValue(propertyTypeAlias);
            }
        }
    }
}