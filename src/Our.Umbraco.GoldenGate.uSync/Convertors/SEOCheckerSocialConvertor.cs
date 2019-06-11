using System.Xml.Linq;

namespace Our.Umbraco.GoldenGate.uSync.Convertors
{
    public class SEOCheckerSocialConvertor : DataTypeConvertor
    {
        public override string GetPropertyAlias()
        {
            return "SEOChecker.SEOCheckerSocialPropertyEditor";
        }

        protected override object ProcessPrevalue(XElement node, string preValueAlias, dynamic config)
        {
            switch (preValueAlias)
            {
                case "mapDescription":
                    config.MapOGDescriptionProperty = GetNodeValue(node);
                    break;

                case "mapImage":
                    config.MapOGImageProperty = GetNodeValue(node);
                    break;

                case "mapTitle":
                    config.MapOGTitleProperty = GetNodeValue(node);
                    break;


                default:
                    throw new System.Exception($"Unknown PreValue alias '{preValueAlias}'");

            }

            return null;
        }
    }
}