using Moq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using SimpleDotNetDB.MySQL.Business;
using SimpleDotNetDB.MySQL.Data;
using SimpleDotNetDB.SQLServer.Business;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using static SimpleDotNetDB.MySQL.Business.MySQLBindingConvertor;

namespace SQLServerBindingConvertorTests
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
            SQLServerBindingConvertor bConvertor = new SQLServerBindingConvertor();
            
            Assert.IsTrue(bConvertor.ToSqlDbType(typeof(bool)) == SqlDbType.Bit);
            Assert.IsTrue(bConvertor.ToSqlDbType(typeof(byte)) == SqlDbType.TinyInt);
            Assert.IsTrue(bConvertor.ToSqlDbType(typeof(byte[])) == SqlDbType.VarBinary);
            Assert.IsTrue(bConvertor.ToSqlDbType(typeof(DateTime)) == SqlDbType.DateTime2);
            Assert.IsTrue(bConvertor.ToSqlDbType(typeof(decimal)) == SqlDbType.Decimal);
            Assert.IsTrue(bConvertor.ToSqlDbType(typeof(double)) == SqlDbType.Float);
            Assert.IsTrue(bConvertor.ToSqlDbType(typeof(Guid)) == SqlDbType.UniqueIdentifier);            
            Assert.IsTrue(bConvertor.ToSqlDbType(typeof(Int16)) == SqlDbType.SmallInt);            
            Assert.IsTrue(bConvertor.ToSqlDbType(typeof(Int32)) == SqlDbType.Int);
            Assert.IsTrue(bConvertor.ToSqlDbType(typeof(Int64)) == SqlDbType.BigInt);
            Assert.IsTrue(bConvertor.ToSqlDbType(typeof(long)) == SqlDbType.BigInt);
            Assert.IsTrue(bConvertor.ToSqlDbType(typeof(object)) == SqlDbType.Variant);
            Assert.IsTrue(bConvertor.ToSqlDbType(typeof(string)) == SqlDbType.VarChar);
            Assert.IsTrue(bConvertor.ToSqlDbType(typeof(DataTable)) == SqlDbType.Structured);
        }
    }
}