using SimpleDotNetDB.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace SimpleDotNetDB.Core.Business
{
    public abstract partial class Wrapper
    {
        protected virtual void ClearSQLCommand()
        {
            SQLCommand.Connection.Close();
            if (SQLCommand.Parameters != null && SQLCommand.Parameters.Count > 0)
            {
                SQLCommand.Parameters.Clear();
            }
            SQLCommand.CommandText = null;
            SQLCommand.Connection = null;
            SQLCommand.CommandTimeout = 0;
            SQLCommand.CommandType = CommandType.Text;
        }

        protected virtual void CommandSetup(string queryOrName, object parameters, object databaseTypes, bool isStoredProcedure)
        {
            //Sets the query or stored procedure name
            SQLCommand.CommandText = queryOrName;

            //Sets the connection to use
            SQLCommand.Connection = Connection;

            //Sets the connection timeout to use
            SQLCommand.CommandTimeout = ConnectionTimeout;

            //Checks if its an stored procedure
            List<Parameter> processedParameters;
            if (isStoredProcedure)
            {
                //if so flags it to the command object
                SQLCommand.CommandType = CommandType.StoredProcedure;

                //extracts and validates any parameters and types we have
                processedParameters = ParameterProcessor.GetParameters(parameters, databaseTypes);
            }
            else
            {
                //extracts and validates any parameters and types we have
                processedParameters = ParameterProcessor.GetParameters(SQLCommand.CommandText, parameters, databaseTypes);
            }

            //Runs the command binding process for all our parameters we have extracted
            ParameterProcessor.BindParameters(SQLCommand, processedParameters);

            //Replaces our internal binding variables with the correct version for the different database queries
            //note: this helps to stop things like email addresses etc being picked up when they should not be
            SQLCommand.CommandText = queryOrName.Replace(BindingStartValue, InternalDatabaseBinding);

            //Opens the connection
            SQLCommand.Connection.Open();
        }

        protected void ExecuteNonQuery(int counter = 0)
        {
            try
            {
                //Executes the command and returns the reader it into the reader
                SQLCommand.ExecuteNonQuery();
            }
            catch (DbException e)
            {
                //Runs the deadlock retry function
                DeadLockRetry(counter, e, ExecuteNonQuery);
            }
        }

        protected DbDataReader ExecuteReader(int counter = 0)
        {
            try
            {
                //Executes the command and returns the reader it into the reader
                return SQLCommand.ExecuteReader();
            }
            catch (DbException e)
            {
                //Runs the deadlock retry function
                return DeadLockRetry(counter, e, ExecuteReader);
            }
        }

        //These methods need to be overriden at the top level as they require specific class information
        protected abstract DbDataAdapter ExecuteDBAdapter(int counter = 0);
        protected abstract void DeadLockRetry(int counter, DbException exception, Action<int> runFunction);
        protected abstract T DeadLockRetry<T>(int counter, DbException exception, Func<int, T> runFunction);
    }
}
