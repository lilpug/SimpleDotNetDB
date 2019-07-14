using System;

namespace SimpleDotNetDB.Core.Data
{
    //This class stores all the parameter information required
    public class Parameter
    {
        public string BindingName { get; set; }
        public object Value { get; set; }
        public Type Type { get; set; }
        public DatabaseType DatabaseType { get; set; }
    }
}
