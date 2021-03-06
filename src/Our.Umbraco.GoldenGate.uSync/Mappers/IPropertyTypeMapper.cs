﻿using System.Collections.Generic;

namespace Our.Umbraco.GoldenGate.uSync.Mappers
{
    public interface IPropertyTypeMapper
    {
        bool IsMapper(string alias);

        string ConvertAlias(string alias);
        string ConvertDatabaseType(string databaseType);
        string ConvertPreValues(IDictionary<string, string> preValues);
    }
}