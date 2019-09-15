using System.Collections.Generic;
using Newtonsoft.Json;

namespace Our.Umbraco.GoldenGate.uSync.Mappers
{
    public abstract class PropertyTypeMapperBase : IPropertyTypeMapper
    {
        public abstract bool IsMapper(string alias);

        public virtual string ConvertAlias(string alias)
        {
            return alias;
        }

        public virtual string ConvertDatabaseType(string databaseType)
        {
            return databaseType;
        }

        public virtual string ConvertPreValues(IDictionary<string, string> preValues)
        {
            return JsonConvert.SerializeObject(preValues);
        }
    }
}