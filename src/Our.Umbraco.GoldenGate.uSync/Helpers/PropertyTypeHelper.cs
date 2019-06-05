using System.Collections.Generic;

namespace Our.Umbraco.GoldenGate.uSync.Helpers
{
    public static class PropertyTypeHelper
    {
        private static IDictionary<string, string> _aliases = new Dictionary<string, string>();

        public static string GetUpdatedAlias(string oldAlias)
        {
            if (_aliases.ContainsKey(oldAlias) == true)
            {
                return _aliases[oldAlias];
            }

            return oldAlias;
        }
    }
}