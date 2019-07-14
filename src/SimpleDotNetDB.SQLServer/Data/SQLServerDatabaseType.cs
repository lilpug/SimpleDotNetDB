using SimpleDotNetDB.Core.Data;
using System;
using System.Data;

namespace SimpleDotNetDB.SQLServer.Data
{
    public class SQLServerDatabaseType : DatabaseType
    {
        public SqlDbType? SQLDBType { get; set; }

        public SQLServerDatabaseType(string bindingName, Type type, object value) : base(type, value)
        {
            if (type == typeof(SqlDbType))
            {
                SQLDBType = (SqlDbType)value;
                TypeFound = true;
            }

            if (!TypeFound)
            {
                throw new Exception($"The parameter type for binding '{bindingName}' is not valid.");
            }
        }
    }
}
