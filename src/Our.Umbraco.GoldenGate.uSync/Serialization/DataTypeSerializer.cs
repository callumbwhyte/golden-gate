using System.Xml.Linq;
using Our.Umbraco.GoldenGate.uSync.Mappers;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using uSync8.Core;
using uSync8.Core.Models;
using uSync8.Core.Serialization;
using Serializers = uSync8.Core.Serialization.Serializers;

namespace Our.Umbraco.GoldenGate.uSync.Serialization
{
    [SyncSerializer("C06E92B7-7440-49B7-B4D2-AF2BF4F3D75D", "DataType Serializer", uSyncConstants.Serialization.DataType)]
    public class DataTypeSerializer : Serializers.DataTypeSerializer, ISyncSerializer<IDataType>
    {
        private readonly MapperFactory _mapperFactory;

        public DataTypeSerializer(IEntityService entityService, ILogger logger, IDataTypeService dataTypeService, MapperFactory mapperFactory)
            : base(entityService, logger, dataTypeService)
        {
            _mapperFactory = mapperFactory;
        }

        public override bool IsValid(XElement node)
        {
            var name = GetName(node);
            var alias = GetAlias(node);
            var databaseType = GetDatabaseType(node);

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
            var name = GetName(node);
            var editorAlias = GetAlias(node);
            var databaseType = GetDatabaseType(node);
            var folder = GetFolder(node);

            var mapper = _mapperFactory.GetPropertyTypeMapper(editorAlias);

            if (mapper != null)
            {
                editorAlias = mapper.ConvertAlias(editorAlias);
                databaseType = mapper.ConvertDatabaseType(databaseType);
            }

            var infoNode = new XElement("Info");

            infoNode.Add(new XElement("Name", name));
            infoNode.Add(new XElement("EditorAlias", editorAlias));
            infoNode.Add(new XElement("DatabaseType", databaseType));
            infoNode.Add(new XElement("Folder", folder));

            node.Add(infoNode);

            node.Add(new XAttribute("Alias", name));

            return base.DeserializeCore(node);
        }

        private string GetName(XElement node)
        {
            return node.Attribute("Name").ValueOrDefault(string.Empty);
        }

        private string GetAlias(XElement node)
        {
            return node.Attribute("Id").ValueOrDefault(string.Empty);
        }

        private string GetDatabaseType(XElement node)
        {
            return node.Attribute("DatabaseType").ValueOrDefault(string.Empty);
        }

        private string GetFolder(XElement node)
        {
            return node.Attribute("Folder").ValueOrDefault(string.Empty);
        }
    }
}