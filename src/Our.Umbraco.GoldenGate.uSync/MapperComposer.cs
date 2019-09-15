using Our.Umbraco.GoldenGate.uSync.Mappers;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Our.Umbraco.GoldenGate.uSync
{
    public class MapperComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<MapperFactory>();
        }
    }
}