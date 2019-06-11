using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Our.Umbraco.GoldenGate.uSync.Convertors
{
    public static class DataTypeConvertorFactory
    {
        private static Dictionary<string, DataTypeConvertor> Convertors;

        public static void RegisterConvertors()
        {
            //This checks current assembly only.
            //Should we add assemblyScanner for ALL dll's (to find custom convertors by other users)
            var types = Assembly.GetAssembly(typeof(DataTypeConvertor)).GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(DataTypeConvertor)));

            Convertors = new Dictionary<string, DataTypeConvertor>();
            foreach (var type in types)
            {
                var convertor = (DataTypeConvertor) Activator.CreateInstance(type);

                Convertors.Add(convertor.GetPropertyAlias(), convertor);
            }
        }

        public static DataTypeConvertor GetConvertor(string alias)
        {
            if (Convertors == null)
            {
                RegisterConvertors();
            }

            if (Convertors.TryGetValue(alias, out var dataTypeConvertor))
            {
                return dataTypeConvertor;
            }

            return null;
        }
    }
}