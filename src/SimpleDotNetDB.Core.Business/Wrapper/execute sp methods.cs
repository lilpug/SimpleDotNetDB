using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace SimpleDotNetDB.Core.Business
{
    public abstract partial class Wrapper
    {
        public void ExecuteStoredProcedureNonQuery(string spName, object parameters)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(spName, parameters, null, true);

                //Executes the query
                ExecuteNonQuery();
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public void ExecuteStoredProcedureNonQuery(string spName, object parameters, object databaseTypes)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(spName, parameters, databaseTypes, true);

                //Executes the query
                ExecuteNonQuery();
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public string ExecuteStoredProcedureReturnString(string spName, object parameters)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(spName, parameters, null, true);

                //Executes the query and returns the reader
                using (DbDataReader reader = ExecuteReader())
                {
                    //Converts the data and sends it back
                    return OutputTypes.ResultToString(reader);
                }
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public string ExecuteStoredProcedureReturnString(string spName, object parameters, object databaseTypes)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(spName, parameters, databaseTypes, true);

                //Executes the query and returns the reader
                using (DbDataReader reader = ExecuteReader())
                {
                    //Converts the data and sends it back
                    return OutputTypes.ResultToString(reader);
                }
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public string ExecuteStoredProcedureReturnJson(string spName, object parameters)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(spName, parameters, null, true);

                //Executes the query and returns the reader
                using (DbDataReader reader = ExecuteReader())
                {
                    //Converts the data and sends it back
                    return OutputTypes.ResultToJson(reader);
                }
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public string ExecuteStoredProcedureReturnJson(string spName, object parameters, object databaseTypes)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(spName, parameters, databaseTypes, true);

                //Executes the query and returns the reader
                using (DbDataReader reader = ExecuteReader())
                {
                    //Converts the data and sends it back
                    return OutputTypes.ResultToJson(reader);
                }
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public string[] ExecuteStoredProcedureReturnStringArray(string spName, object parameters)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(spName, parameters, null, true);

                //Executes the query and returns the reader
                using (DbDataReader reader = ExecuteReader())
                {
                    //Converts the data and sends it back
                    return OutputTypes.ResultToStringArray(reader);
                }
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public string[] ExecuteStoredProcedureReturnStringArray(string spName, object parameters, object databaseTypes)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(spName, parameters, databaseTypes, true);

                //Executes the query and returns the reader
                using (DbDataReader reader = ExecuteReader())
                {
                    //Converts the data and sends it back
                    return OutputTypes.ResultToStringArray(reader);
                }
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public List<dynamic> ExecuteStoredProcedureReturnDynamicList(string spName, object parameters)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(spName, parameters, null, true);

                //Executes the query and returns the reader
                using (DbDataReader reader = ExecuteReader())
                {
                    //Converts the data and sends it back
                    return OutputTypes.ResultToDynamic(reader);
                }
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public List<dynamic> ExecuteStoredProcedureReturnDynamicList(string spName, object parameters, object databaseTypes)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(spName, parameters, databaseTypes, true);

                //Executes the query and returns the reader
                using (DbDataReader reader = ExecuteReader())
                {
                    //Converts the data and sends it back
                    return OutputTypes.ResultToDynamic(reader);
                }
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public DataTable ExecuteStoredProcedureReturnDataTable(string spName, object parameters)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(spName, parameters, null, true);

                //Executes the query and returns the reader
                using (DbDataReader reader = ExecuteReader())
                {
                    //Converts the data and sends it back
                    return OutputTypes.ResultToDataTable(reader);
                }
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public DataTable ExecuteStoredProcedureReturnDataTable(string spName, object parameters, object databaseTypes)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(spName, parameters, databaseTypes, true);

                //Executes the query and returns the reader
                using (DbDataReader reader = ExecuteReader())
                {
                    //Converts the data and sends it back
                    return OutputTypes.ResultToDataTable(reader);
                }
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public DataSet ExecuteStoredProcedureReturnDataSet(string spName, object parameters, bool enforceConstraints)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(spName, parameters, null, true);

                //Executes the query and returns the adapter
                using (DbDataAdapter reader = ExecuteDBAdapter())
                {
                    //Converts the data and sends it back
                    return OutputTypes.ResultToDataSet(reader, enforceConstraints);
                }
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public DataSet ExecuteStoredProcedureReturnDataSet(string spName, object parameters, object databaseTypes, bool enforceConstraints)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(spName, parameters, databaseTypes, true);

                //Executes the query and returns the adapter
                using (DbDataAdapter reader = ExecuteDBAdapter())
                {
                    //Converts the data and sends it back
                    return OutputTypes.ResultToDataSet(reader, enforceConstraints);
                }
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public List<T> ExecuteStoredProcedureReturn<T>(string spName, object parameters, object databaseTypes)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(spName, parameters, databaseTypes, true);

                //Executes the query and returns the reader
                using (DbDataReader reader = ExecuteReader())
                {
                    //Converts the data and sends it back
                    return OutputTypes.ResultsToCast<T>(reader);
                }
            }
            finally
            {
                ClearSQLCommand();
            }
        }
    }
}
