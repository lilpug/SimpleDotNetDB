using SimpleDotNetDB.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimpleDotNetDB.Core.Business
{
    public abstract class ParameterProcessor
    {
        protected string BindingStartValue { get; set; }
        protected string InternalDatabaseBinding { get; set; }
        protected Regex BindingRegex { get; set; }
        

        public ParameterProcessor(string internalDatabaseBinding)
        {
            BindingStartValue = "@@";
            InternalDatabaseBinding = internalDatabaseBinding;
            BindingRegex = new Regex($@"{BindingStartValue}(.+?)(\x20|,|\z|;|[(]|[)]|=|\""|\')");
        }
        public ParameterProcessor(string internalDatabaseBinding, string bindingStartValue)
        {      
            BindingStartValue = bindingStartValue;
            InternalDatabaseBinding = internalDatabaseBinding;
            BindingRegex = new Regex($@"{BindingStartValue}(.+?)(\x20|,|\z|;|[(]|[)]|=|\""|\')");
        }


        //This is the core function for extracting and validating parameters and types for a stored procedure
        public List<Parameter> GetParameters(object parameters, object databaseTypes = null)
        {
            //Extracts the parameters from the objects and/or query provided
            List<Parameter> results;
           
            //If its the stored procedure route we don't need a SQL string being supplied
            if (databaseTypes != null)
            {
                results = GetSPParameterBindings(parameters, databaseTypes);
            }
            else
            {
                results = GetSPParameterBindings(parameters);
            }
           
            return results;
        }

        //This is the core function for extracting and validating parameters and types for an SQL query
        public List<Parameter> GetParameters(string query, object parameters, object databaseTypes = null)
        {
            //Extracts the parameters from the objects and/or query provided
            List<Parameter> results;
           
            //If its the SQL route then supply the string for validation checks
            if (databaseTypes != null)
            {
                results = GetSQLParameterBindings(query, parameters, databaseTypes);
            }
            else
            {
                results = GetSQLParameterBindings(query, parameters);
            }            
            return results;
        }

        //This function obtains a list of parameters with their bindings and checks their validation
        protected List<Parameter> GetSPParameterBindings(object parameters)
        {
            List<Parameter> results = new List<Parameter>();

            //Gets all the properties supplied
            var properties = parameters?.GetType()?.GetProperties();

            //If we have properties loop over them and process them
            if (properties != null && properties.Length > 0)
            {
                //Gets each of the properties and checks if its matched against our binding values
                foreach (var prop in properties)
                {
                    string propertyName = prop.Name;
                    object propertyValue = prop.GetValue(parameters, null);
                    Type propertyType = propertyValue.GetType();
                    results.Add(new Parameter() { BindingName = propertyName, Type = propertyType, Value = propertyValue });
                }
            }

            return results;
        }

        //This function obtains a list of parameters with their bindings and checks their validation, it also adds the database types supplied
        protected List<Parameter> GetSPParameterBindings(object parameters, object databaseTypes)
        {
            //Calls the main process
            List<Parameter> results = GetSPParameterBindings(parameters);

            //Attachs the database type process
            return AddDatabaseTypes(results, databaseTypes);            
        }

        //This function obtains a list of parameters with their bindings and checks their validation
        protected List<Parameter> GetSQLParameterBindings(string sql, object parameters)
        {
            List<Parameter> results = new List<Parameter>();

            //This section deals with processing the bindings
            List<string> bindingIndexs = new List<string>();
            if (!string.IsNullOrWhiteSpace(sql))
            {   
                //Pulls out the parameters                
                bindingIndexs = BindingRegex.Matches(sql).Cast<Match>().Select(s => s.Groups[1].Value).Distinct().ToList();   
            }

            //Gets all the properties supplied
            var properties = parameters?.GetType()?.GetProperties();

            //Check if the property count does not match the binding index count and if so throw an exception
            if ( (properties == null && bindingIndexs.Count > 0) || (properties != null && bindingIndexs.Count != properties.Length) )
            {
                throw new Exception("The number of parameter binding indexs in the SQL does not match the parameters suplied.");
            }

            //If we have properties loop over them and process them
            if (properties != null && properties.Length > 0)
            {
                //Gets each of the properties and checks if its matched against our binding values
                foreach (var prop in properties)
                {
                    string propertyName = prop.Name;
                    object propertyValue = prop.GetValue(parameters, null);
                    Type propertyType = propertyValue.GetType();

                    if (bindingIndexs.Contains(propertyName))
                    {
                        results.Add(new Parameter() { BindingName = propertyName, Type = propertyType, Value = propertyValue });
                    }
                }

                //Checks that once we have finished if we still have the same counts
                //Note: if not then one of the names does not match the binding indexs in the SQL
                if (results.Count != bindingIndexs.Count)
                {
                    throw new Exception("The number of binding indexs in your SQL matches the supplied parameters but one or more of them does not have the correct matching name.");
                }
            }

            return results;
        }

        //This function obtains a list of parameters with their bindings and checks their validation, it also adds the database types supplied
        protected List<Parameter> GetSQLParameterBindings(string sql, object parameters, object databaseTypes)
        {
            //Calls the main process
            List<Parameter> results = GetSQLParameterBindings(sql, parameters);

            //Attachs the database type process
            return AddDatabaseTypes(results, databaseTypes);
        }

        //This function is used to attach database types onto the parameter results
        protected List<Parameter> AddDatabaseTypes(List<Parameter> results, object databaseTypes)
        {
            //Gets all the properties supplied for the database types
            var properties = databaseTypes?.GetType()?.GetProperties();

            //Check if the property count does not match the binding index count and if so throw an exception
            if ((properties == null && results.Count > 0) || (properties != null && results.Count != properties.Length))
            {
                throw new Exception("The number of parameters does not match the supplied databaseTypes.");
            }

            //If we have properties loop over them and process them
            if (properties != null && properties.Length > 0)
            {
                int matchCounter = 0;

                //Gets each of the properties and checks if its matched against our binding values
                foreach (var prop in properties)
                {
                    string propertyName = prop.Name;
                    object propertyValue = prop.GetValue(databaseTypes, null);
                    Type propertyType = propertyValue.GetType();

                    var item = results.FirstOrDefault(i => i.BindingName == propertyName);
                    if (item != null)
                    {
                        AddParameterDatabaseType(item, propertyName, propertyType, propertyValue);                        
                        matchCounter++;
                    }
                }

                //Checks that once we have finished adding all the database types if we still have the same counts
                //Note: if not then one of the names for the database types does not match the results.
                if (results.Count != matchCounter)
                {
                    throw new Exception("One or more of the databaseTypes does not have a matching binding name to the other parameters.");
                }
            }

            return results;
        }

        //This function is a dummy hook which allows us to polymorphism our database types between MySQL and SQL Server deppending which library your using
        protected virtual void AddParameterDatabaseType(Parameter parameter, string propertyName, Type propertyType, object propertyValue)
        {   
        }
        public virtual void BindParameters(DbCommand command, List<Parameter> results)
        {
        }
    }
}
