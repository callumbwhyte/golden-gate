using Umbraco.Core.Composing;
using uSync8.Core;

namespace Our.Umbraco.GoldenGate.uSync
{
    [ComposeAfter(typeof(uSyncCoreComposer))]
    public class SerializerComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {

        }
    }
}