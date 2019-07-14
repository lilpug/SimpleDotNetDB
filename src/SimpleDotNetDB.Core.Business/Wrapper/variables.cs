using System;
using System.Data;
using System.Data.Common;

namespace SimpleDotNetDB.Core.Business
{
    public abstract partial class Wrapper : IDisposable
    {   
        protected DbConnection Connection { get; set; }
        protected DbCommand SQLCommand { get; set; }
        protected ParameterProcessor ParameterProcessor { get; set; }
        protected OutputTypes OutputTypes { get; set; }
        protected int MaxDeadlockRetry { get; set; }
        protected int MaxDeadlockMSDelay { get; set; }
        protected int ConnectionTimeout { get; set; }
        protected string DBVersion { get; set; }

        //This is used to tell us what variables are being inserted and need to be bound via SQL or inside a stored procedure
        protected string BindingStartValue { get; set; }

        //This is used to convert the variable binding and SQL queries to the real value for that particular database version
        protected string InternalDatabaseBinding { get; set; }

        public Wrapper()
        {
            OutputTypes = new OutputTypes();
            MaxDeadlockRetry = 3;
            MaxDeadlockMSDelay = 1000;
            DBVersion = null;
            ConnectionTimeout = 30;

            //These values are set to default as they work for both SQL Server and MySQL
            BindingStartValue = "@@";
            InternalDatabaseBinding = "@";
        }

        public virtual void Dispose()
        {
            if(Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }

            Connection = null;
            SQLCommand = null;
            ParameterProcessor = null;
            OutputTypes = null;
            MaxDeadlockMSDelay = 0;
            MaxDeadlockRetry = 0;
            ConnectionTimeout = 0;
            DBVersion = null;
            BindingStartValue = null;
            InternalDatabaseBinding = null;
        }
    }
}
