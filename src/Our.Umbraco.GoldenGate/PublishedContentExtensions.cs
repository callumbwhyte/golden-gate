using Umbraco.Core.Models;

namespace Our.Umbraco.GoldenGate
{
    public static class PublishedContentExtensions
    {
        public static object Value(this IPublishedContent content, string alias)
        {
            var property = content.GetProperty(alias);

            if (property != null)
            {
                return property.Value;
            }

            return null;
        }
    }
}