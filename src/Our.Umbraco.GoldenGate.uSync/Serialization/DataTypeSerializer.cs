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
    public class DataTypeSerializer : Serializers.DataTypeSerializer
    {
        private readonly MapperFactory _mapperFactory;

        public DataTypeSerializer(IEntityService entityService, ILogger logger, IDataTypeService dataTypeService, MapperFactory mapperFactory)
            : base(entityService, logger, dataTypeService)
        {
            _mapperFactory = mapperFactory;
        }

        public override bool IsValid(XElement node)
        {
            var name = node.GetAttribute("Name");
            var alias = node.GetAttribute("Id");
            var databaseType = node.GetAttribute("DatabaseType");

            if (node.Name.LocalName == this.ItemType
                && name != string.Empty
                && alias != string.Empty
                && databaseType != string.Empty)
            {
                return true;
            }

            return base.IsValid(node);
        }

        protected override SyncAttempt<IDataType> DeserializeCore(XElement node)
        {
            var name = node.GetAttribute("Name");
            var editorAlias = node.GetAttribute("Id");
            var databaseType = node.GetAttribute("DatabaseType");
            var folder = node.GetAttribute("Folder");
            var settings = node.GetElement("PreValues");

            var mapper = _mapperFactory.GetPropertyTypeMapper(editorAlias);

            if (mapper != null)
            {
                editorAlias = mapper.ConvertAlias(editorAlias);
                databaseType = mapper.ConvertDatabaseType(databaseType);

                MapPreValues(settings, mapper);
            }

            var infoNode = new XElement("Info");

            infoNode.AddElement("Name", name);
            infoNode.AddElement("EditorAlias", editorAlias);
            infoNode.AddElement("DatabaseType", databaseType);
            infoNode.AddElement("Folder", folder);

            node.Add(infoNode);

            node.AddAttribute("Alias", name);

            return base.DeserializeCore(node);
        }

        private void MapPreValues(XElement settings, IPropertyTypeMapper mapper)
        {
            var preValues = settings.GetElements("PreValue")
                .ToDictionary(x => x.GetAttribute("Alias"), x => x.Value);

            if (preValues.Any() == true)
            {
                settings.SetValue(mapper.ConvertPreValues(preValues));
            }
        }
    }
}