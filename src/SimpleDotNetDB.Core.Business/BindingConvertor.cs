using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SimpleDotNetDB.Core.Business
{
    public class BindingConvertor
    {
        /// <summary>
        /// This variable stores a list of the type mapper class
        /// </summary>
        public List<DBTypeMapEntry> TypeList { get; set; }

        /// <summary>
        /// This variable stores the current supplied database version
        /// </summary>
        protected string DBVersion { get; set; }

        /// <summary>
        /// This variable stores if a dbversion variable has been passed
        /// </summary>
        protected bool HasDBVersion { get; set; }

        /// <summary>
        /// This is the constructor for the SQL Server type mapper
        /// </summary>
        public BindingConvertor()
        {
            TypeList = new List<DBTypeMapEntry>();

            DBVersion = null;
            HasDBVersion = false;

            LoadTypes();
        }

        public BindingConvertor(string dbVersion)
        {
            TypeList = new List<DBTypeMapEntry>();

            DBVersion = dbVersion;

            //Stores if the db version has been supplied
            //Note: this is done to save processing time on linq checks
            if (!string.IsNullOrWhiteSpace(this.DBVersion))
            {
                HasDBVersion = true;
            }

            LoadTypes();
        }
             
        /// <summary>
        /// This function is the loading function for the types
        /// </summary>
        protected virtual void LoadTypes()
        {
            TypeList.Add(new DBTypeMapEntry(typeof(bool), DbType.Boolean));
            TypeList.Add(new DBTypeMapEntry(typeof(byte), DbType.Binary));
            TypeList.Add(new DBTypeMapEntry(typeof(byte[]), DbType.Binary));
            TypeList.Add(new DBTypeMapEntry(typeof(DateTime), DbType.DateTime2));
            TypeList.Add(new DBTypeMapEntry(typeof(Decimal), DbType.Decimal));
            TypeList.Add(new DBTypeMapEntry(typeof(double), DbType.Double));
            TypeList.Add(new DBTypeMapEntry(typeof(Guid), DbType.Guid));
            TypeList.Add(new DBTypeMapEntry(typeof(Int16), DbType.Int16));
            TypeList.Add(new DBTypeMapEntry(typeof(Int32), DbType.Int32));
            TypeList.Add(new DBTypeMapEntry(typeof(Int64), DbType.Int64));
            TypeList.Add(new DBTypeMapEntry(typeof(long), DbType.Int64));
            TypeList.Add(new DBTypeMapEntry(typeof(object), DbType.Object));
            TypeList.Add(new DBTypeMapEntry(typeof(string), DbType.String));
            TypeList.Add(new DBTypeMapEntry(typeof(DataTable), DbType.Object));
        }
        
        protected DBTypeMapEntry Find(Type type)
        {
            //Checks if the type exists in the list
            var temp = from item in TypeList.AsEnumerable()
                       where item.Type == type && (item.DBVersion == null || item.DBVersion.Length == 0 || (HasDBVersion && item.DBVersion.Contains(DBVersion)))
                       select item;

            //Checks if we have atleast one entry
            if (temp != null && temp.FirstOrDefault() != null)
            {
                //Returns it if so
                return temp.First();
            }
            else
            {
                throw new ApplicationException("Database Type Converter: data was used which is an unsupported Type");
            }
        }

        public class DBTypeMapEntry
        {
            /// <summary>
            /// Stores the database versions which can use this type
            /// Note: null means all!
            /// </summary>
            public string[] DBVersion { get; set; }

            /// <summary>
            /// Stores the .NET reference type
            /// </summary>
            public Type Type { get; set; }

            /// <summary>
            /// Stores the standard database reference type
            /// </summary>
            public DbType DbType { get; set; }


            public DBTypeMapEntry(Type type, DbType dbType, params string[] dbVersions)
            {
                Type = type;
                DbType = dbType;
                DBVersion = dbVersions;
            }
        }
    }
}
