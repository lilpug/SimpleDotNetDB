using SimpleDotNetDB.Core.Business;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace SimpleDotNetDB.MySQL.Business
{
    public class MySQLBindingConvertor : BindingConvertor
    {   
        public MySQLBindingConvertor() : base()
        {
        }
        public MySQLBindingConvertor(string dbVersion) : base(dbVersion)
        {
        }

        //This function loads the MySQL types
        protected override void LoadTypes()
        {
            TypeList.Add(new MySQLDBTypeMapEntry(typeof(bool), DbType.Boolean, MySqlDbType.Bit));
            TypeList.Add(new MySQLDBTypeMapEntry(typeof(byte), DbType.Binary, MySqlDbType.Byte));
            TypeList.Add(new MySQLDBTypeMapEntry(typeof(byte[]), DbType.Binary, MySqlDbType.VarBinary));
            TypeList.Add(new MySQLDBTypeMapEntry(typeof(DateTime), DbType.DateTime2, MySqlDbType.DateTime));
            TypeList.Add(new MySQLDBTypeMapEntry(typeof(Decimal), DbType.Decimal, MySqlDbType.Decimal));
            TypeList.Add(new MySQLDBTypeMapEntry(typeof(double), DbType.Double, MySqlDbType.Float));
            TypeList.Add(new MySQLDBTypeMapEntry(typeof(Guid), DbType.Guid, MySqlDbType.Guid));
            TypeList.Add(new MySQLDBTypeMapEntry(typeof(Int16), DbType.Int16, MySqlDbType.Int16));
            TypeList.Add(new MySQLDBTypeMapEntry(typeof(Int32), DbType.Int32, MySqlDbType.Int32));
            TypeList.Add(new MySQLDBTypeMapEntry(typeof(Int64), DbType.Int64, MySqlDbType.Int64));
            TypeList.Add(new MySQLDBTypeMapEntry(typeof(long), DbType.Int64, MySqlDbType.Int64));
            TypeList.Add(new MySQLDBTypeMapEntry(typeof(object), DbType.Object, MySqlDbType.Blob));
            TypeList.Add(new MySQLDBTypeMapEntry(typeof(string), DbType.String, MySqlDbType.VarChar));
            TypeList.Add(new MySQLDBTypeMapEntry(typeof(DataTable), DbType.Object, MySqlDbType.Blob));
        }

        //This function returns the database type for the .net type supplied
        public MySqlDbType? ToMySqlDbType(Type type)
        {
            DBTypeMapEntry entry = Find(type);
            if (entry != null)
            {
                MySQLDBTypeMapEntry item = entry as MySQLDBTypeMapEntry;
                return item.MySQLDbType;
            }
            return null;
        }

        //Maps the new type into the DBTypeEntry class
        public class MySQLDBTypeMapEntry : DBTypeMapEntry
        {
            /// <summary>
            /// Stores the MySQL database reference type
            /// </summary>
            public MySqlDbType MySQLDbType { get; set; }
            
            public MySQLDBTypeMapEntry(Type type, DbType dbType, MySqlDbType mySqlDbType, params string[] dbVersions) : base(type, dbType, dbVersions)
            {
                MySQLDbType = mySqlDbType;                
            }
        }
    }
}
