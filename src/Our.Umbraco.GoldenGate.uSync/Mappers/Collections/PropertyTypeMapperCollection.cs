using System.Collections.Generic;
using Umbraco.Core.Composing;

namespace Our.Umbraco.GoldenGate.uSync.Mappers.Collections
{
    public class PropertyTypeMapperCollection : BuilderCollectionBase<IPropertyTypeMapper>
    {
        public PropertyTypeMapperCollection(IEnumerable<IPropertyTypeMapper> mappers)
            : base(mappers)
        {

        }
    }

    public class PropertyTypeMapperCollectionBuilder : LazyCollectionBuilderBase<PropertyTypeMapperCollectionBuilder, PropertyTypeMapperCollection, IPropertyTypeMapper>
    {
        protected override PropertyTypeMapperCollectionBuilder This => this;
    }
}