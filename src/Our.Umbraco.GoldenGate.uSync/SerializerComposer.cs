using Our.Umbraco.GoldenGate.uSync.Serialization;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Models;
using uSync8.Core;
using uSync8.Core.Serialization;

namespace Our.Umbraco.GoldenGate.uSync
{
    [ComposeAfter(typeof(uSyncCoreComposer))]
    public class SerializerComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.RegisterUnique<ISyncSerializer<IContentType>, ContentTypeSerializer>();
            composition.RegisterUnique<ISyncSerializer<ITemplate>, TemplateSerializer>();
        }
    }
}