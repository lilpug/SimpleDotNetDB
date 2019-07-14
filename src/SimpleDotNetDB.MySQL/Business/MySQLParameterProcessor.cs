using SimpleDotNetDB.Core.Business;
using SimpleDotNetDB.Core.Data;
using SimpleDotNetDB.MySQL.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace SimpleDotNetDB.MySQL.Business
{
    public class MySQLParameterProcessor : ParameterProcessor
    {
        public MySQLBindingConvertor BindingConvertor { get; set; }

        public MySQLParameterProcessor(string internalDatabaseBinding) : base(internalDatabaseBinding)
        {
            BindingConvertor = new MySQLBindingConvertor();
        }
        public MySQLParameterProcessor(string internalDatabaseBinding, string dbVersion) : base(internalDatabaseBinding)
        {
            if (!string.IsNullOrWhiteSpace(dbVersion))
            {
                BindingConvertor = new MySQLBindingConvertor(dbVersion);
            }
            else
            {
                BindingConvertor = new MySQLBindingConvertor();
            }
        }
        public MySQLParameterProcessor(string internalDatabaseBinding, string dbVersion, string bindingStartValue) : base(internalDatabaseBinding, bindingStartValue)
        {
            if (!string.IsNullOrWhiteSpace(dbVersion))
            {
                BindingConvertor = new MySQLBindingConvertor(dbVersion);
            }
            else
            {
                BindingConvertor = new MySQLBindingConvertor();
            }
        }

        //Overrides the updating of the database type to store our MySQL version
        protected override void AddParameterDatabaseType(Parameter parameter, string propertyName, Type propertyType, object propertyValue)
        {
            MySQLDatabaseType databaseType = new MySQLDatabaseType(propertyName, propertyType, propertyValue);
            parameter.DatabaseType = databaseType;
        }

        //Overrides the binding process so we can use the correct database types
        public override void BindParameters(DbCommand command, List<Parameter> results)
        {
            if (results != null)
            {
                //Morphs the command object intot he sql version
                MySqlCommand sqlCommand = command as MySqlCommand;

                foreach (Parameter param in results)
                {
                    //Overrides the binding if its an sp variable
                    string bindingStart = (sqlCommand.CommandType == CommandType.StoredProcedure) ? "" : InternalDatabaseBinding;

                    string bindingDefinition = $"{bindingStart}{param.BindingName}";
                    sqlCommand.Parameters.AddWithValue(bindingDefinition, (param.Value ?? DBNull.Value));
                    if (param.Value != null)
                    {
                        if (param.DatabaseType != null)
                        {
                            MySQLDatabaseType dbType = param.DatabaseType as MySQLDatabaseType;
                            sqlCommand.Parameters[bindingDefinition].MySqlDbType = dbType.MySQLDBType.Value;
                        }
                        else//Use our automatic conversion system
                        {
                            MySqlDbType? dbType = BindingConvertor.ToMySqlDbType(param.Type);
                            if(dbType.HasValue)
                            {
                                sqlCommand.Parameters[bindingDefinition].MySqlDbType = dbType.Value;
                            }
                        }
                    }
                }
            }
        }
    }
}
