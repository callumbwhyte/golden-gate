using System.Xml.Linq;

namespace Our.Umbraco.GoldenGate.uSync.Convertors
{
    public class SEOCheckerConvertor : DataTypeConvertor
    {
        public override string GetPropertyAlias()
        {
            return "SEOChecker";
        }

        protected override object ProcessPrevalue(XElement node, string preValueAlias, dynamic config)
        {
            switch (preValueAlias)
            {
                case "Keywords":
                    config.MapKeywordsProperty = GetNodeValue(node);
                    break;

                case "mapDescription":
                    config.MapDescriptionProperty = GetNodeValue(node);
                    break;

                case "mapTitle":
                    config.MapTitleProperty = GetNodeValue(node);
                    break;

                case "useKeywordTag":
                    config.UseKeywordTag = "1".Equals(GetNodeValue(node));
                    break;

                case "validationMode":
                    var mode = GetNodeValue(node);
                    switch (mode)
                    {
                        case "Never validate automatically":
                            config.ValidationMode = "never";
                            break;

                        case "Always validate automatically":
                            config.ValidationMode = "always";
                            break;

                        case "Automatically validate after save":
                            config.ValidationMode = "aftersave";
                            break;
                    }
                    
                    break;

                default:
                    throw new System.Exception($"Unknown PreValue alias '{preValueAlias}'");

            }

            return null;
        }
    }
}