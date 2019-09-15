using Our.Umbraco.GoldenGate.uSync.Mappers.Collections;
using Umbraco.Core.Composing;

namespace Our.Umbraco.GoldenGate.uSync.Composing
{
    public static class ComposerExtensions
    {
        public static PropertyTypeMapperCollectionBuilder PropertyTypeMappers(this Composition composition)
        {
            return composition.WithCollectionBuilder<PropertyTypeMapperCollectionBuilder>();
        }
    }
}