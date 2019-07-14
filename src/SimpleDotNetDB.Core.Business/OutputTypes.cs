using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.IO;
using System.Text;

namespace SimpleDotNetDB.Core.Business
{
    public class OutputTypes
    {
        public virtual string ResultToJson(IDataReader myReader)
        {   
            if (myReader == null || myReader.FieldCount == 0)
            {   
                return null;
            }

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartArray();

                while (myReader.Read())
                {
                    jsonWriter.WriteStartObject();

                    int fields = myReader.FieldCount;

                    for (int i = 0; i < fields; i++)
                    {
                        jsonWriter.WritePropertyName(myReader.GetName(i));
                        jsonWriter.WriteValue(myReader[i]);
                    }

                    jsonWriter.WriteEndObject();
                }

                jsonWriter.WriteEndArray();

                return sw.ToString();
            }
        }

        public virtual List<T> ResultsToCast<T>(IDataReader myReader)
        {
            string json = ResultToJson(myReader);
            if (!string.IsNullOrWhiteSpace(json))
            {
                return JsonConvert.DeserializeObject<List<T>>(json);
            }
            return default(List<T>);
        }

        public virtual string ResultToString(IDataReader myReader)
        {
            //Puts the first output into a string
            string value = null;
            if (myReader != null && myReader.Read())
            {
                value = myReader.GetValue(0).ToString();
            }
            return value;
        }

        public virtual DataTable ResultToDataTable(IDataReader myReader)
        {
            DataTable main_store = new DataTable();
            //Loads the data into a datatable format
            main_store.Load(myReader);
            return main_store;
        }

        public virtual DataSet ResultToDataSet(IDataAdapter myAdapter, bool enforceConstraints)
        {
            DataSet ds = new DataSet
            {
                //Sets the constraints setup
                EnforceConstraints = enforceConstraints
            };

            //Loads the data into a datatable format
            myAdapter.Fill(ds);

            return ds;
        }

        public virtual string[] ResultToStringArray(IDataReader myReader)
        {
            List<string> ls = new List<string>();

            while (myReader.Read())
            {
                ls.Add(myReader.GetValue(0).ToString());
            }

            return ls.ToArray();
        }
        
        public virtual List<dynamic> ResultToDynamic(IDataReader myReader)
        {
            //Builds the temporary storage object
            List<dynamic> temp = new List<dynamic>();

            //Loops over the reader row by row
            while (myReader.Read())
            {
                //Creates the expandoObject for this row
                dynamic main = new ExpandoObject();

                //This uses the main expandoObject as a reference to add new properties to it without knowing the name at design time
                var expandoObject = main as IDictionary<string, object>;

                //Loops over the columns and values for this particular row
                for (int i = 0; i < myReader.FieldCount; i++)
                {
                    expandoObject.Add(myReader.GetName(i), myReader[i]);
                }

                //Return the original dynamic object now we have the propeties added
                temp.Add(main);
            }

            //Returns the new dynamic list
            return temp;
        }

    }
}
