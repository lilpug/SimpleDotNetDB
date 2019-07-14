using FastMember;
using Moq;
using NUnit.Framework;
using SimpleDotNetDB.Core.Business;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace OutputTypeTests
{
    public class ResultTypes
    {
        //used to test type casting values
        private class TestRecord
        {
            public string you { get; set; }
            public int AGE { get; set; }
        }

        private IDataReader Reader { get; set; }        
        private OutputTypes OutputTypes { get; set; }

        [SetUp]
        public void SetupReaders()
        {
            Reader = MockIDataReader();            
            OutputTypes = new OutputTypes();
        }
                
        [Test]
        public void ResultToJson()
        {   
            string json = "[{\"you\":\"Dave\",\"AGE\":18},{\"you\":\"Bob\",\"AGE\":19},{\"you\":\"John\",\"AGE\":20}]";

            OutputTypes outputTypes = new OutputTypes();
            string val = outputTypes.ResultToJson(Reader);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(val));
            Assert.IsTrue(val == json);
        }

        [Test]
        public void ResultsToCast()
        {
            List<TestRecord> records = new List<TestRecord>();            

            records = OutputTypes.ResultsToCast<TestRecord>(Reader);

            Assert.IsTrue(records.Count == 3);
            Assert.IsTrue(records[0].you == "Dave" && records[0].AGE == 18);
            Assert.IsTrue(records[1].you == "Bob" && records[1].AGE == 19);
            Assert.IsTrue(records[2].you == "John" && records[2].AGE == 20);
        }

        [Test]
        public void ResultToString()
        {
            Assert.IsTrue(OutputTypes.ResultToString(Reader) == "Dave");
        }

        [Test]
        public void ResultToDataTable()
        {
            DataTable dt = OutputTypes.ResultToDataTable(Reader);

            Assert.IsTrue(dt != null && dt.Rows.Count == 3);
            Assert.IsTrue(dt.Rows[0]["you"].ToString() == "Dave" && dt.Rows[0]["AGE"].ToString() == "18");
            Assert.IsTrue(dt.Rows[1]["you"].ToString() == "Bob" && dt.Rows[1]["AGE"].ToString() == "19");
            Assert.IsTrue(dt.Rows[2]["you"].ToString() == "John" && dt.Rows[2]["AGE"].ToString() == "20");
        }

        [Test]
        public void ResultToDataSet()
        {
            //left this out for now as due to how hard it is to mock we would be simply testing our own test mock case which is not the end solution.
        }

        [Test]
        public void ResultToStringArray()
        {
            string[] records = OutputTypes.ResultToStringArray(Reader);

            Assert.IsTrue(records.Length == 3);
            Assert.IsTrue(records[0] == "Dave");
            Assert.IsTrue(records[1] == "Bob");
            Assert.IsTrue(records[2] == "John");
        }

        [Test]
        public void ResultToDynamic()
        {
            var dynObject = OutputTypes.ResultToDynamic(Reader);

            Assert.IsTrue(dynObject.Count == 3);
            Assert.IsTrue(dynObject[0].you == "Dave" && dynObject[0].AGE == 18);
            Assert.IsTrue(dynObject[1].you == "Bob" && dynObject[1].AGE == 19);
            Assert.IsTrue(dynObject[2].you == "John" && dynObject[2].AGE == 20);

        }
        
        private IDataReader MockIDataReader()
        {
            IList<TestRecord> ls = new List<TestRecord>();
            ls.Add(new TestRecord { you = "Dave", AGE = 18 });
            ls.Add(new TestRecord { you = "Bob", AGE = 19 });
            ls.Add(new TestRecord { you = "John", AGE = 20 });

            return ObjectReader.Create(ls, "you", "AGE");
        }
    }
}