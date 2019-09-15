using Our.Umbraco.GoldenGate.uSync.Composing;
using Our.Umbraco.GoldenGate.uSync.Mappers;
using Our.Umbraco.GoldenGate.uSync.Mappers.Implement;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Our.Umbraco.GoldenGate.uSync
{
    public class MapperComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.PropertyTypeMappers().Add<CorePropertyTypeMapper>();

            composition.Register<MapperFactory>();
        }
    }
}