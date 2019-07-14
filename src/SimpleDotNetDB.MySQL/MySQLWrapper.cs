using SimpleDotNetDB.Core.Business;
using SimpleDotNetDB.MySQL.Business;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Threading;

namespace SimpleDotNetDB.MySQL
{
    public class MySQLWrapper : Wrapper
    {
        protected MySQLWrapper() : base()
        {
            SQLCommand = new MySqlCommand();                                                                    
            ParameterProcessor = new MySQLParameterProcessor(InternalDatabaseBinding);            
        }
        public MySQLWrapper(string connectionString) : this()
        {
            Connection = new MySqlConnection(connectionString);
        }
        public MySQLWrapper(string connectionString, WrapperAdditionalConfig config) : this()
        {
            Connection = new MySqlConnection(connectionString);

            if (!string.IsNullOrWhiteSpace(config.ParameterBindingStartValue))
            {
                BindingStartValue = config.ParameterBindingStartValue;
                if (!string.IsNullOrWhiteSpace(config.DatabaseVersion))
                {
                    ParameterProcessor = new MySQLParameterProcessor(InternalDatabaseBinding, config.DatabaseVersion, config.ParameterBindingStartValue);
                }
                else
                {
                    ParameterProcessor = new MySQLParameterProcessor(InternalDatabaseBinding, null, config.ParameterBindingStartValue);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(config.DatabaseVersion))
                {
                    ParameterProcessor = new MySQLParameterProcessor(InternalDatabaseBinding, config.DatabaseVersion);
                }
            }

            MaxDeadlockRetry = config?.MaxDeadlockRetry ?? 0;
            MaxDeadlockMSDelay = config?.MaxDeadlockMSDelay ?? 0;
            ConnectionTimeout = config?.ConnectionTimeout ?? 30;
        }
        public MySQLWrapper(MySqlConnection connectionObject) : this()
        {
            Connection = connectionObject;            
        }
        public MySQLWrapper(MySqlConnection connectionObject, WrapperAdditionalConfig config) : this()
        {
            Connection = connectionObject;

            if (!string.IsNullOrWhiteSpace(config.ParameterBindingStartValue))
            {
                BindingStartValue = config.ParameterBindingStartValue;
                if (!string.IsNullOrWhiteSpace(config.DatabaseVersion))
                {
                    ParameterProcessor = new MySQLParameterProcessor(InternalDatabaseBinding, config.DatabaseVersion, config.ParameterBindingStartValue);
                }
                else
                {
                    ParameterProcessor = new MySQLParameterProcessor(InternalDatabaseBinding, null, config.ParameterBindingStartValue);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(config.DatabaseVersion))
                {
                    ParameterProcessor = new MySQLParameterProcessor(InternalDatabaseBinding, config.DatabaseVersion);
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
                return new MySqlDataAdapter(SQLCommand as MySqlCommand);
            }
            catch (MySqlException e)
            {
                //Runs the deadlock retry function
                return DeadLockRetry(counter, e, ExecuteDBAdapter);
            }
        }

        protected override void DeadLockRetry(int counter, DbException exception, Action<int> runFunction)
        {
            MySqlException sException = exception as MySqlException;

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
            MySqlException sException = exception as MySqlException;

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
