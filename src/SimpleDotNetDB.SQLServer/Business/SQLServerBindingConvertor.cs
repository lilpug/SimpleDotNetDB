using SimpleDotNetDB.Core.Business;
using System;
using System.Data;

namespace SimpleDotNetDB.SQLServer.Business
{
    public class SQLServerBindingConvertor : BindingConvertor
    {
        public SQLServerBindingConvertor() : base()
        {
        }
        public SQLServerBindingConvertor(string dbVersion) : base(dbVersion)
        {
        }

        //This function loads the SQL Server types
        protected override void LoadTypes()
        {
            TypeList.Add(new SqlServerDbTypeMapEntry(typeof(bool), DbType.Boolean, SqlDbType.Bit));
            TypeList.Add(new SqlServerDbTypeMapEntry(typeof(byte), DbType.Binary, SqlDbType.TinyInt));
            TypeList.Add(new SqlServerDbTypeMapEntry(typeof(byte[]), DbType.Binary, SqlDbType.VarBinary));
            TypeList.Add(new SqlServerDbTypeMapEntry(typeof(DateTime), DbType.DateTime2, SqlDbType.DateTime2));
            TypeList.Add(new SqlServerDbTypeMapEntry(typeof(Decimal), DbType.Decimal, SqlDbType.Decimal));
            TypeList.Add(new SqlServerDbTypeMapEntry(typeof(double), DbType.Double, SqlDbType.Float));
            TypeList.Add(new SqlServerDbTypeMapEntry(typeof(Guid), DbType.Guid, SqlDbType.UniqueIdentifier));
            TypeList.Add(new SqlServerDbTypeMapEntry(typeof(Int16), DbType.Int16, SqlDbType.SmallInt));
            TypeList.Add(new SqlServerDbTypeMapEntry(typeof(Int32), DbType.Int32, SqlDbType.Int));
            TypeList.Add(new SqlServerDbTypeMapEntry(typeof(Int64), DbType.Int64, SqlDbType.BigInt));
            TypeList.Add(new SqlServerDbTypeMapEntry(typeof(long), DbType.Int64, SqlDbType.BigInt));
            TypeList.Add(new SqlServerDbTypeMapEntry(typeof(object), DbType.Object, SqlDbType.Variant));
            TypeList.Add(new SqlServerDbTypeMapEntry(typeof(string), DbType.String, SqlDbType.VarChar));
            TypeList.Add(new SqlServerDbTypeMapEntry(typeof(DataTable), DbType.Object, SqlDbType.Structured));
        }

        //This function returns the database type for the .net type supplied
        public SqlDbType? ToSqlDbType(Type type)
        {
            DBTypeMapEntry entry = Find(type);
            if (entry != null)
            {
                SqlServerDbTypeMapEntry item = entry as SqlServerDbTypeMapEntry;
                return item.SQLserverDbType;
            }
            return null;
        }

        //Maps the new type into the DBTypeEntry class
        public class SqlServerDbTypeMapEntry : DBTypeMapEntry
        {
            /// <summary>
            /// Stores the SQL Server database reference type
            /// </summary>
            public SqlDbType SQLserverDbType { get; set; }
            
            public SqlServerDbTypeMapEntry(Type type, DbType dbType, SqlDbType sqlServerDbType, params string[] dbVersions) : base(type, dbType, dbVersions)
            {   
                SQLserverDbType = sqlServerDbType;                
            }
        }
    }
}
