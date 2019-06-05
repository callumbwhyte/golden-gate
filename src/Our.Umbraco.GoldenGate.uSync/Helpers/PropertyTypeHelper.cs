using System.Collections.Generic;
using Umbraco.Core;

namespace Our.Umbraco.GoldenGate.uSync.Helpers
{
    public static class PropertyTypeHelper
    {
        private static IDictionary<string, string> _aliases = new Dictionary<string, string>()
        {
            { "Umbraco.ColorPickerAlias", Constants.PropertyEditors.Aliases.ColorPicker },
            { "Umbraco.ContentPicker2", Constants.PropertyEditors.Aliases.ContentPicker },
            { "Umbraco.Date", Constants.PropertyEditors.Aliases.DateTime },
            { "Umbraco.NoEdit", Constants.PropertyEditors.Aliases.Label },
            { "Umbraco.MediaPicker2", Constants.PropertyEditors.Aliases.MediaPicker },
            { "Umbraco.MemberPicker2", Constants.PropertyEditors.Aliases.MemberPicker },
            { "Umbraco.MultiNodeTreePicker2", Constants.PropertyEditors.Aliases.MultiNodeTreePicker },
            { "Umbraco.RelatedLinks2", Constants.PropertyEditors.Aliases.MultiUrlPicker },
            { "Umbraco.TinyMCEv3", Constants.PropertyEditors.Aliases.TinyMce },
            { "Umbraco.TextboxMultiple", Constants.PropertyEditors.Aliases.TextArea }
        };

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