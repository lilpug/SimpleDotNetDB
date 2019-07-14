using SimpleDotNetDB.Core.Business;
using SimpleDotNetDB.Core.Data;
using SimpleDotNetDB.SQLServer.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace SimpleDotNetDB.SQLServer.Business
{
    public class SQLServerParameterProcessor : ParameterProcessor
    {
        public SQLServerBindingConvertor BindingConvertor { get; set; }

        public SQLServerParameterProcessor(string internalDatabaseBinding) : base(internalDatabaseBinding)
        {
            BindingConvertor = new SQLServerBindingConvertor();
        }

        public SQLServerParameterProcessor(string internalDatabaseBinding, string dbVersion) : base(internalDatabaseBinding)
        {
            if (!string.IsNullOrWhiteSpace(dbVersion))
            {
                BindingConvertor = new SQLServerBindingConvertor(dbVersion);
            }
            else
            {
                BindingConvertor = new SQLServerBindingConvertor();
            }
        }
        public SQLServerParameterProcessor(string internalDatabaseBinding, string dbVersion, string bindingStartValue) : base(internalDatabaseBinding, bindingStartValue)
        {
            if (!string.IsNullOrWhiteSpace(dbVersion))
            {
                BindingConvertor = new SQLServerBindingConvertor(dbVersion);
            }
            else
            {
                BindingConvertor = new SQLServerBindingConvertor();
            }
        }

        //Overrides the updating of the database type to store our MySQL version
        protected override void AddParameterDatabaseType(Parameter parameter, string propertyName, Type propertyType, object propertyValue)
        {
            SQLServerDatabaseType databaseType = new SQLServerDatabaseType(propertyName, propertyType, propertyValue);
            parameter.DatabaseType = databaseType;
        }

        //Overrides the binding process so we can use the correct database types
        public override void BindParameters(DbCommand command, List<Parameter> results)
        {
            if (results != null)
            {
                //Morphs the command object intot he sql version
                SqlCommand sqlCommand = command as SqlCommand;

                foreach (Parameter param in results)
                {
                    //Overrides the binding if its an sp variable
                    string bindingStart = (sqlCommand.CommandType == CommandType.StoredProcedure) ? "@" : InternalDatabaseBinding;

                    string bindingDefinition = $"{bindingStart}{param.BindingName}";
                    sqlCommand.Parameters.AddWithValue(bindingDefinition, (param.Value ?? DBNull.Value));
                    if (param.Value != null)
                    {
                        if (param.DatabaseType != null)
                        {
                            SQLServerDatabaseType dbType = param.DatabaseType as SQLServerDatabaseType;
                            sqlCommand.Parameters[bindingDefinition].SqlDbType = dbType.SQLDBType.Value;
                        }
                        else//Use our automatic conversion system
                        {
                            SqlDbType? dbType = BindingConvertor.ToSqlDbType(param.Type);
                            if (dbType.HasValue)
                            {
                                sqlCommand.Parameters[bindingDefinition].SqlDbType = dbType.Value;
                            }
                        }
                    }
                }
            }
        }
    }
}
