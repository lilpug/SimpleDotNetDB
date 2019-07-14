using SimpleDotNetDB.Core.Data;
using MySql.Data.MySqlClient;
using System;

namespace SimpleDotNetDB.MySQL.Data
{
    public class MySQLDatabaseType : DatabaseType
    {
        public MySqlDbType? MySQLDBType { get; set; }

        public MySQLDatabaseType(string bindingName, Type type, object value) : base(type, value)
        {
            if (type == typeof(MySqlDbType))
            {
                MySQLDBType = (MySqlDbType)value;
                TypeFound = true;
            }

            if (!TypeFound)
            {
                throw new Exception($"The parameter type for binding '{bindingName}' is not valid.");
            }
        }
    }
}
