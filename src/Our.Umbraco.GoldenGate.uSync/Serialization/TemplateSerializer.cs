using System;
using System.Xml.Linq;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using uSync8.Core;
using uSync8.Core.Extensions;
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
            var key = GetKey(node);
            var alias = GetAlias(node);

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
            var key = GetKey(node);
            var alias = GetAlias(node);
            var parent = GetParent(node);

            node.Add(new XAttribute("Key", key));
            node.Add(new XAttribute("Alias", alias));

            node.Add(new XElement("Parent", parent));

            return base.DeserializeCore(node);
        }

        private Guid GetKey(XElement node)
        {
            return node.Element("Key").ValueOrDefault(Guid.Empty);
        }

        private string GetAlias(XElement node)
        {
            return node.Element("Alias").ValueOrDefault(string.Empty);
        }

        private string GetParent(XElement node)
        {
            return node.Element("Master").ValueOrDefault(string.Empty);
        }
    }
}