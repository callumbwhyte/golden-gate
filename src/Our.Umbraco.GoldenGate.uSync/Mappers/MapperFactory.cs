using System.Linq;
using Our.Umbraco.GoldenGate.uSync.Mappers.Collections;

namespace Our.Umbraco.GoldenGate.uSync.Mappers
{
    public class MapperFactory
    {
        private readonly PropertyTypeMapperCollection _PropertyTypeMappers;

        public MapperFactory(PropertyTypeMapperCollection PropertyTypeMappers)
        {
            _PropertyTypeMappers = PropertyTypeMappers;
        }

        public IPropertyTypeMapper GetPropertyTypeMapper(string alias)
        {
            return _PropertyTypeMappers.FirstOrDefault(x => x.IsMapper(alias));
        }
    }
}