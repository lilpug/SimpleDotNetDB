using SimpleDotNetDB.Core.Business;
using SimpleDotNetDB.SQLServer.Business;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;

namespace SimpleDotNetDB.SQLServer
{
    public class SQLServerWrapper : Wrapper
    {
        protected SQLServerWrapper() : base()
        {
            SQLCommand = new SqlCommand();                                                                     
            ParameterProcessor = new SQLServerParameterProcessor(InternalDatabaseBinding);
        }
        public SQLServerWrapper(string connectionString) : this()
        {
            Connection = new SqlConnection(connectionString);
        }
        public SQLServerWrapper(string connectionString, WrapperAdditionalConfig config) : this()
        {
            Connection = new SqlConnection(connectionString);

            if (!string.IsNullOrWhiteSpace(config.ParameterBindingStartValue))
            {
                BindingStartValue = config.ParameterBindingStartValue;
                if (!string.IsNullOrWhiteSpace(config.DatabaseVersion))
                {
                    ParameterProcessor = new SQLServerParameterProcessor(InternalDatabaseBinding, config.DatabaseVersion, config.ParameterBindingStartValue);
                }
                else
                {
                    ParameterProcessor = new SQLServerParameterProcessor(InternalDatabaseBinding, null, config.ParameterBindingStartValue);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(config.DatabaseVersion))
                {
                    ParameterProcessor = new SQLServerParameterProcessor(InternalDatabaseBinding, config.DatabaseVersion);
                }
            }

            MaxDeadlockRetry = config?.MaxDeadlockRetry ?? 0;
            MaxDeadlockMSDelay = config?.MaxDeadlockMSDelay ?? 0;
            ConnectionTimeout = config?.ConnectionTimeout ?? 30;
        }
        public SQLServerWrapper(string connectionString, SqlCredential credentials) : this()
        {
            Connection = new SqlConnection(connectionString, credentials);            
        }
        public SQLServerWrapper(string connectionString, SqlCredential credentials, WrapperAdditionalConfig config) : this()
        {
            Connection = new SqlConnection(connectionString, credentials);

            if (!string.IsNullOrWhiteSpace(config.ParameterBindingStartValue))
            {
                BindingStartValue = config.ParameterBindingStartValue;
                if (!string.IsNullOrWhiteSpace(config.DatabaseVersion))
                {
                    ParameterProcessor = new SQLServerParameterProcessor(InternalDatabaseBinding, config.DatabaseVersion, config.ParameterBindingStartValue);
                }
                else
                {
                    ParameterProcessor = new SQLServerParameterProcessor(InternalDatabaseBinding, null, config.ParameterBindingStartValue);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(config.DatabaseVersion))
                {
                    ParameterProcessor = new SQLServerParameterProcessor(InternalDatabaseBinding, config.DatabaseVersion);
                }
            }

            MaxDeadlockRetry = config?.MaxDeadlockRetry ?? 0;
            MaxDeadlockMSDelay = config?.MaxDeadlockMSDelay ?? 0;
            ConnectionTimeout = config?.ConnectionTimeout ?? 30;
        }
        public SQLServerWrapper(SqlConnection connectionObject) : this()
        {
            Connection = connectionObject;            
        }
        public SQLServerWrapper(SqlConnection connectionObject, WrapperAdditionalConfig config) : this()
        {
            Connection = connectionObject;

            if (!string.IsNullOrWhiteSpace(config.ParameterBindingStartValue))
            {
                BindingStartValue = config.ParameterBindingStartValue;
                if (!string.IsNullOrWhiteSpace(config.DatabaseVersion))
                {
                    ParameterProcessor = new SQLServerParameterProcessor(InternalDatabaseBinding, config.DatabaseVersion, config.ParameterBindingStartValue);
                }
                else
                {
                    ParameterProcessor = new SQLServerParameterProcessor(InternalDatabaseBinding, null, config.ParameterBindingStartValue);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(config.DatabaseVersion))
                {
                    ParameterProcessor = new SQLServerParameterProcessor(InternalDatabaseBinding, config.DatabaseVersion);
                }
            }

            MaxDeadlockRetry = config?.MaxDeadlockRetry ?? 0;
            MaxDeadlockMSDelay = config?.MaxDeadlockMSDelay ?? 0;
            ConnectionTimeout = config?.ConnectionTimeout ?? 30;
        }

        protected override DbDataAdapter ExecuteDBAdapter(int counter = 0)
        {
            try
            {
                //Executes the command and returns the data adapter
                return new SqlDataAdapter(SQLCommand as SqlCommand);
            }
            catch (SqlException e)
            {
                //Runs the deadlock retry function
                return DeadLockRetry(counter, e, ExecuteDBAdapter);
            }
        }

        protected override void DeadLockRetry(int counter, DbException exception, Action<int> runFunction)
        {
            SqlException sException = exception as SqlException;

            //Deadlock issue: re-attempt based on the number of tries we can do
            if (sException.Number == 1205 && counter < MaxDeadlockRetry)
            {
                //Wait for the set amount of time before trying again
                Thread.Sleep(MaxDeadlockMSDelay);

                //Run the retry function
                runFunction(counter + 1);
            }
            throw exception;
        }

        protected override T DeadLockRetry<T>(int counter, DbException exception, Func<int, T> runFunction)
        {
            SqlException sException = exception as SqlException;

            //Deadlock issue: re-attempt based on the number of tries we can do
            if (sException.Number == 1205 && counter < MaxDeadlockRetry)
            {
                //Wait for the set amount of time before trying again
                Thread.Sleep(MaxDeadlockMSDelay);

                //Run the retry function
                return runFunction(counter + 1);
            }     
            throw exception;
        }
    }
}
