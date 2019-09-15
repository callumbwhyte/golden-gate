using System;

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

        public virtual object ConvertPreValues(object preValues)
        {
            return preValues;
        }
    }
}