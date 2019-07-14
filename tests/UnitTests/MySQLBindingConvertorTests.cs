using Moq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using SimpleDotNetDB.MySQL.Business;
using SimpleDotNetDB.MySQL.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using static SimpleDotNetDB.MySQL.Business.MySQLBindingConvertor;

namespace MySQLBindingConvertorTests
{

    /*Mock<MySQLParameterProcessor> chk = new Mock<MySQLParameterProcessor>();
    chk.Setup(x => x.).Returns(true);*/

    /*[SetUp]
    public void MySQLParameterProcessor_SQLExtractionSuccess()
    {

    }*/

    public class Core
    {
        [Test]
        public void TestingAllTypeConversions()
        {   
            MySQLBindingConvertor bConvertor = new MySQLBindingConvertor();
            
            Assert.IsTrue(bConvertor.ToMySqlDbType(typeof(bool)) == MySqlDbType.Bit);
            Assert.IsTrue(bConvertor.ToMySqlDbType(typeof(byte)) == MySqlDbType.Byte);
            Assert.IsTrue(bConvertor.ToMySqlDbType(typeof(byte[])) == MySqlDbType.VarBinary);
            Assert.IsTrue(bConvertor.ToMySqlDbType(typeof(DateTime)) == MySqlDbType.DateTime);
            Assert.IsTrue(bConvertor.ToMySqlDbType(typeof(decimal)) == MySqlDbType.Decimal);
            Assert.IsTrue(bConvertor.ToMySqlDbType(typeof(double)) == MySqlDbType.Float);
            Assert.IsTrue(bConvertor.ToMySqlDbType(typeof(Guid)) == MySqlDbType.Guid);            
            Assert.IsTrue(bConvertor.ToMySqlDbType(typeof(Int16)) == MySqlDbType.Int16);            
            Assert.IsTrue(bConvertor.ToMySqlDbType(typeof(Int32)) == MySqlDbType.Int32);
            Assert.IsTrue(bConvertor.ToMySqlDbType(typeof(Int64)) == MySqlDbType.Int64);
            Assert.IsTrue(bConvertor.ToMySqlDbType(typeof(long)) == MySqlDbType.Int64);
            Assert.IsTrue(bConvertor.ToMySqlDbType(typeof(object)) == MySqlDbType.Blob);
            Assert.IsTrue(bConvertor.ToMySqlDbType(typeof(string)) == MySqlDbType.VarChar);
            Assert.IsTrue(bConvertor.ToMySqlDbType(typeof(DataTable)) == MySqlDbType.Blob);
        }
    }
}