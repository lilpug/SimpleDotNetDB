using Moq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using SimpleDotNetDB.MySQL.Business;
using SimpleDotNetDB.MySQL.Data;
using System;
using System.Data.Common;
using System.Linq;

namespace MySQLParameterProcessorTests
{

    /*Mock<MySQLParameterProcessor> chk = new Mock<MySQLParameterProcessor>();
    chk.Setup(x => x.).Returns(true);*/

    /*[SetUp]
    public void MySQLParameterProcessor_SQLExtractionSuccess()
    {

    }*/

    public class SQLExtraction
    {
        private const string query = "SELECT * FROM TableOne where A = @@you and B = @@AGE;";

        [Test]
        public void Success()
        {            
            var pms = new { you = "test123", AGE = 11 };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            var parameters = mProcessor.GetParameters(query, pms);
            Assert.IsTrue(parameters != null && parameters.Count == 2);
        }

        [Test]
        public void SuccessWithTypes()
        {   
            var pms = new { you = "test123", AGE = 11 };
            var typs = new { you = MySqlDbType.Text, AGE = MySqlDbType.Int32 };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            var parameters = mProcessor.GetParameters(query, pms, typs);
            Assert.IsTrue(parameters != null && parameters.Count == 2);
        }

        [Test]
        public void SuccessWithTypesCheck()
        {   
            var pms = new { you = "test123", AGE = 11 };
            var typs = new { you = MySqlDbType.Text, AGE = MySqlDbType.Int32 };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            var parameters = mProcessor.GetParameters(query, pms, typs);

            Assert.IsTrue(parameters != null && parameters.Count == 2);

            MySQLDatabaseType youType = parameters.Where(p => p.BindingName == "you").First().DatabaseType as MySQLDatabaseType;
            MySQLDatabaseType AGEType = parameters.Where(p => p.BindingName == "AGE").First().DatabaseType as MySQLDatabaseType;

            Assert.IsTrue(youType.MySQLDBType == MySqlDbType.Text && AGEType.MySQLDBType == MySqlDbType.Int32);

        }

        [Test]
        public void FailureMissingParameter()
        {   
            var pms = new { you = "test123" };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            Assert.Throws(Is.TypeOf<Exception>().And.Message.EqualTo("The number of parameter binding indexs in the SQL does not match the parameters suplied."), delegate { mProcessor.GetParameters(query, pms); });
        }

        [Test]
        public void FailureWrongParameterName()
        {   
            var pms = new { you = "test123", AGEE = 11 };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            Assert.Throws(Is.TypeOf<Exception>().And.Message.EqualTo("The number of binding indexs in your SQL matches the supplied parameters but one or more of them does not have the correct matching name."), delegate { mProcessor.GetParameters(query, pms); });
        }

        [Test]
        public void FailureMissingType()
        {   
            var pms = new { you = "test123", AGE = 11 };
            var typs = new { you = MySqlDbType.Text };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");            

            Assert.Throws(Is.TypeOf<Exception>().And.Message.EqualTo("The number of parameters does not match the supplied databaseTypes."), delegate { mProcessor.GetParameters(query, pms, typs); });
        }

        [Test]
        public void FailureWrongTypeName()
        {   
            var pms = new { you = "test123", AGE = 11 };
            var typs = new { you = MySqlDbType.Text, AGEE = MySqlDbType.Int32 };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            Assert.Throws(Is.TypeOf<Exception>().And.Message.EqualTo("One or more of the databaseTypes does not have a matching binding name to the other parameters."), delegate { mProcessor.GetParameters(query, pms, typs); });
        }

        [Test]
        public void SuccessfulBindingParameter()
        {   
            var pms = new { you = "test123", AGE = 11 };            

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            var results = mProcessor.GetParameters(query, pms);

            var dbCommand = new MySqlCommand();

            mProcessor.BindParameters(dbCommand, results);

            Assert.IsTrue(dbCommand.Parameters != null && dbCommand.Parameters.Count == 2);
            Assert.IsTrue(dbCommand.Parameters["@you"]?.Value?.ToString() == "test123");
            Assert.IsTrue(Convert.ToInt32(dbCommand.Parameters["@AGE"]?.Value?.ToString()) == 11);
        }
        
        [Test]
        public void SuccessfulBindingParameterTypes()
        {   
            var pms = new { you = "test123", AGE = 11 };            

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            var results = mProcessor.GetParameters(query, pms);

            var dbCommand = new MySqlCommand();

            mProcessor.BindParameters(dbCommand, results);

            Assert.IsTrue(dbCommand.Parameters != null && dbCommand.Parameters.Count == 2);
            Assert.IsTrue(dbCommand.Parameters["@you"].MySqlDbType == MySqlDbType.VarChar);
            Assert.IsTrue(dbCommand.Parameters["@AGE"].MySqlDbType == MySqlDbType.Int32);
        }

        [Test]
        public void SuccessfulBindingParameterWithTypes()
        {   
            var pms = new { you = "test123", AGE = 11 };
            var typs = new { you = MySqlDbType.Text, AGE = MySqlDbType.Int32 };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            var results = mProcessor.GetParameters(query, pms, typs);

            var dbCommand = new MySqlCommand();

            mProcessor.BindParameters(dbCommand, results);

            Assert.IsTrue(dbCommand.Parameters != null && dbCommand.Parameters.Count == 2);
            Assert.IsTrue(dbCommand.Parameters["@you"]?.Value?.ToString() == "test123");
            Assert.IsTrue(Convert.ToInt32(dbCommand.Parameters["@AGE"]?.Value?.ToString()) == 11);
        }
        
        [Test]
        public void SuccessfulBindingParameterTypesWithTypes()
        {   
            var pms = new { you = "test123", AGE = 11 };
            var typs = new { you = MySqlDbType.Text, AGE = MySqlDbType.Int32 };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            var results = mProcessor.GetParameters(query, pms, typs);

            var dbCommand = new MySqlCommand();

            mProcessor.BindParameters(dbCommand, results);

            Assert.IsTrue(dbCommand.Parameters != null && dbCommand.Parameters.Count == 2);
            Assert.IsTrue(dbCommand.Parameters["@you"].MySqlDbType == MySqlDbType.Text);
            Assert.IsTrue(dbCommand.Parameters["@AGE"].MySqlDbType == MySqlDbType.Int32);
        }
    }

    public class SPExtraction
    {
        [Test]
        public void Success()
        {   
            var pms = new { you = "test123", AGE = 11 };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            var parameters = mProcessor.GetParameters(pms);
            Assert.IsTrue(parameters != null && parameters.Count == 2);
        }

        [Test]
        public void SuccessWithTypes()
        {   
            var pms = new { you = "test123", AGE = 11 };
            var typs = new { you = MySqlDbType.Text, AGE = MySqlDbType.Int32 };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            var parameters = mProcessor.GetParameters(pms, typs);
            Assert.IsTrue(parameters != null && parameters.Count == 2);
        }

        [Test]
        public void SuccessWithTypesCheck()
        {   
            var pms = new { you = "test123", AGE = 11 };
            var typs = new { you = MySqlDbType.Text, AGE = MySqlDbType.Int32 };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            var parameters = mProcessor.GetParameters(pms, typs);

            Assert.IsTrue(parameters != null && parameters.Count == 2);

            MySQLDatabaseType youType = parameters.Where(p => p.BindingName == "you").First().DatabaseType as MySQLDatabaseType;
            MySQLDatabaseType AGEType = parameters.Where(p => p.BindingName == "AGE").First().DatabaseType as MySQLDatabaseType;

            Assert.IsTrue(youType.MySQLDBType == MySqlDbType.Text && AGEType.MySQLDBType == MySqlDbType.Int32);

        }

        [Test]
        public void FailureMissingType()
        {   
            var pms = new { you = "test123", AGE = 11 };
            var typs = new { you = MySqlDbType.Text };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");

            Assert.Throws(Is.TypeOf<Exception>().And.Message.EqualTo("The number of parameters does not match the supplied databaseTypes."), delegate { mProcessor.GetParameters(pms, typs); });
        }

        [Test]
        public void FailureWrongTypeName()
        {   
            var pms = new { you = "test123", AGE = 11 };
            var typs = new { you = MySqlDbType.Text, AGEE = MySqlDbType.Int32 };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            Assert.Throws(Is.TypeOf<Exception>().And.Message.EqualTo("One or more of the databaseTypes does not have a matching binding name to the other parameters."), delegate { mProcessor.GetParameters(pms, typs); });
        }

        [Test]
        public void SuccessfulBindingParameter()
        {   
            var pms = new { you = "test123", AGE = 11 };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            var results = mProcessor.GetParameters(pms);

            var dbCommand = new MySqlCommand();

            mProcessor.BindParameters(dbCommand, results);

            Assert.IsTrue(dbCommand.Parameters != null && dbCommand.Parameters.Count == 2);
            Assert.IsTrue(dbCommand.Parameters["@you"]?.Value?.ToString() == "test123");
            Assert.IsTrue(Convert.ToInt32(dbCommand.Parameters["@AGE"]?.Value?.ToString()) == 11);
        }


        [Test]
        public void SuccessfulBindingParameterTypes()
        {   
            var pms = new { you = "test123", AGE = 11 };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            var results = mProcessor.GetParameters(pms);

            var dbCommand = new MySqlCommand();

            mProcessor.BindParameters(dbCommand, results);

            Assert.IsTrue(dbCommand.Parameters != null && dbCommand.Parameters.Count == 2);
            Assert.IsTrue(dbCommand.Parameters["@you"].MySqlDbType == MySqlDbType.VarChar);
            Assert.IsTrue(dbCommand.Parameters["@AGE"].MySqlDbType == MySqlDbType.Int32);
        }

        [Test]
        public void SuccessfulBindingParameterWithTypes()
        {   
            var pms = new { you = "test123", AGE = 11 };
            var typs = new { you = MySqlDbType.Text, AGE = MySqlDbType.Int32 };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            var results = mProcessor.GetParameters(pms, typs);

            var dbCommand = new MySqlCommand();

            mProcessor.BindParameters(dbCommand, results);

            Assert.IsTrue(dbCommand.Parameters != null && dbCommand.Parameters.Count == 2);
            Assert.IsTrue(dbCommand.Parameters["@you"]?.Value?.ToString() == "test123");
            Assert.IsTrue(Convert.ToInt32(dbCommand.Parameters["@AGE"]?.Value?.ToString()) == 11);
        }


        [Test]
        public void SuccessfulBindingParameterTypesWithTypes()
        {   
            var pms = new { you = "test123", AGE = 11 };
            var typs = new { you = MySqlDbType.Text, AGE = MySqlDbType.Int32 };

            MySQLParameterProcessor mProcessor = new MySQLParameterProcessor("@");
            var results = mProcessor.GetParameters(pms, typs);

            var dbCommand = new MySqlCommand();

            mProcessor.BindParameters(dbCommand, results);

            Assert.IsTrue(dbCommand.Parameters != null && dbCommand.Parameters.Count == 2);
            Assert.IsTrue(dbCommand.Parameters["@you"].MySqlDbType == MySqlDbType.Text);
            Assert.IsTrue(dbCommand.Parameters["@AGE"].MySqlDbType == MySqlDbType.Int32);
        }
    }
}