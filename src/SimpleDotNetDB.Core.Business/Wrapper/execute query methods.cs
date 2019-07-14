using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Text;

namespace SimpleDotNetDB.Core.Business
{
    public abstract partial class Wrapper
    {
        public void ExecuteNonQuery(string sql, object parameters)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(sql, parameters, null, false);

                //Executes the query
                ExecuteNonQuery();
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public void ExecuteNonQuery(string sql, object parameters, object databaseTypes)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(sql, parameters, databaseTypes, false);

                //Executes the query
                ExecuteNonQuery();
            }
            finally
            {
                ClearSQLCommand();
            }
        }

        public string ExecuteQueryReturnString(string sql, object parameters)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(sql, parameters, null, false);

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

        public string ExecuteQueryReturnString(string sql, object parameters, object databaseTypes)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(sql, parameters, databaseTypes, false);

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

        public string ExecuteQueryReturnJson(string sql, object parameters)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(sql, parameters, null, false);

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

        public string ExecuteQueryReturnJson(string sql, object parameters, object databaseTypes)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(sql, parameters, databaseTypes, false);

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

        public string[] ExecuteQueryReturnStringArray(string sql, object parameters)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(sql, parameters, null, false);

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

        public string[] ExecuteQueryReturnStringArray(string sql, object parameters, object databaseTypes)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(sql, parameters, databaseTypes, false);

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

        public List<dynamic> ExecuteQueryReturnDynamicList(string sql, object parameters)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(sql, parameters, null, false);

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

        public List<dynamic> ExecuteQueryReturnDynamicList(string sql, object parameters, object databaseTypes)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(sql, parameters, databaseTypes, false);

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

        public DataTable ExecuteQueryReturnDataTable(string sql, object parameters)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(sql, parameters, null, false);

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

        public DataTable ExecuteQueryReturnDataTable(string sql, object parameters, object databaseTypes)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(sql, parameters, databaseTypes, false);

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

        public DataSet ExecuteQueryReturnDataSet(string sql, object parameters, bool enforceConstraints)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(sql, parameters, null, false);

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

        public DataSet ExecuteQueryReturnDataSet(string sql, object parameters, object databaseTypes, bool enforceConstraints)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(sql, parameters, databaseTypes, false);

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

        public List<T> ExecuteQueryReturn<T>(string sql, object parameters, object databaseTypes)
        {
            try
            {
                //Sets up the command object with all the required processing
                CommandSetup(sql, parameters, databaseTypes, false);

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
