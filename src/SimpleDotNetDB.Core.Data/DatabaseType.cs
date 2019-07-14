using System;
using System.Data;

namespace SimpleDotNetDB.Core.Data
{
    //This class stores the processed database types against a binding
    public abstract class DatabaseType
    {
        public DatabaseType(Type type, object value)
        {
            if (type == typeof(DbType))
            {
                DBType = (DbType)value;
                TypeFound = true;
            }            
        }
        protected bool TypeFound { get; set; }       
        public DbType? DBType { get; set; }
    }
}
