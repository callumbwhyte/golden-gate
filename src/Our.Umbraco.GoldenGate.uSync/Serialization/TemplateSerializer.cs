using System;
using System.Xml.Linq;
using Our.Umbraco.GoldenGate.uSync.Extensions;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using uSync8.Core;
using uSync8.Core.Models;
using uSync8.Core.Serialization;
using Serializers = uSync8.Core.Serialization.Serializers;

namespace Our.Umbraco.GoldenGate.uSync.Serialization
{
    [SyncSerializer("D0E0769D-CCAE-47B4-AD34-4182C587B08A", "Template Serializer", uSyncConstants.Serialization.Template)]
    public class TemplateSerializer : Serializers.TemplateSerializer, ISyncSerializer<ITemplate>
    {
        public TemplateSerializer(IEntityService entityService, ILogger logger, IFileService fileService)
            : base(entityService, logger, fileService)
        {

        }

        public override bool IsValid(XElement node)
        {
            var key = node.GetValue<Guid>("Key");
            var alias = node.GetValue("Alias");

            if (node.Name.LocalName == ItemType
                && key != Guid.Empty
                && alias != string.Empty)
            {
                return true;
            }

            return base.IsValid(node);
        }

        protected override SyncAttempt<ITemplate> DeserializeCore(XElement node)
        {
            var key = node.GetValue<Guid>("Key");
            var alias = node.GetValue("Alias");
            var parent = node.GetValue("Master");

            node.AddAttribute("Key", key);
            node.AddAttribute("Alias", alias);

            node.AddElement("Parent", parent);

            return base.DeserializeCore(node);
        }
    }
}