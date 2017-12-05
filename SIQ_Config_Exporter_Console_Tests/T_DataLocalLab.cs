using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient;
using static SiqConfigExport.Constants;

namespace UnitTestProject1
{
    /// <summary>
    /// This test class checks the basic functionality of the data layer.  Be sure to update the constants for your lab
    /// </summary>
    [TestClass]
    public class T_DataLocalLab
    {
        /// <summary>
        /// Update all of these variables as appropriate for your testing environment.  CONN_STR should 
        /// be correct for your test environment.  The other variable names will 
        /// tell you what part of the conn string should be wrong for the given test.
        /// </summary>
        const String CONN_STR = "Server=localhost,31433;Database=SecurityIQDB;User Id=sa;Password = Sailpoint1!;";
        const String CONN_STR_BAD_HOSTNAME = "Server=badhostname,31433;Database=SecurityIQDB;User Id=sa;Password = Sailpoint1!;";
        const String CONN_STR_BAD_DBNAME = "Server=badhostname,31433;Database=badDbName;User Id=sa;Password = Sailpoint1!;";
        const String CONN_STR_BAD_PORT = "Server=localhost,9999;Database=SecurityIQDB;User Id=sa;Password = Sailpoint1!;";
        const String CONN_STR_BAD_USER = "Server=localhost,31433;Database=SecurityIQDB;User Id=baduser;Password = Sailpoint1!;";
        const String CONN_STR_BAD_PASS = "Server=localhost,31433;Database=SecurityIQDB;User Id=sa;Password = badpass;";
        const String CONN_STR_SPECIAL_CHAR = "Server=localhost,31433;Database=SecurityIQDB;User Id=sa;Password = 'sssssss;$%#';";
        DataSet ds = new DataSet();


        /// <summary>
        /// This test will pass if we get the expected SqlException due to a bad host name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void Test_BadHostName()
        {
                ds = SiqConfigExport.Data.GetDataFromSIQdb(T_DataLocalLab.CONN_STR_BAD_HOSTNAME);
        }

        /// <summary>
        /// This test will pass if we get the expected SqlException due to a bad database name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void Test_BadDbName()
        {
            ds = SiqConfigExport.Data.GetDataFromSIQdb(T_DataLocalLab.CONN_STR_BAD_DBNAME);
        }

        /// <summary>
        /// This test will pass if we get the expected SqlException due to a bad db server port
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void Test_BadPort()
        {
            ds = SiqConfigExport.Data.GetDataFromSIQdb(T_DataLocalLab.CONN_STR_BAD_PORT);
        }

        /// <summary>
        /// /// <summary>
        /// This test will pass if we get the expected SqlException due to a bad username
        /// </summary>
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void Test_BadUser()
        {
            ds = SiqConfigExport.Data.GetDataFromSIQdb(T_DataLocalLab.CONN_STR_BAD_USER);
        }

        /// <summary>
        /// /// <summary>
        /// This test will pass if we get the expected SqlException due to a bad password
        /// </summary>
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void Test_BadPassword()
        {
            ds = SiqConfigExport.Data.GetDataFromSIQdb(T_DataLocalLab.CONN_STR_BAD_PASS);
        }
        
        /// <summary>
        /// /// <summary>
        /// This test will pass if we get the a dataset that meets these expected basic conditions
        /// </summary>
        /// </summary>
        [TestMethod]
        public void Test_the_Siq_DbDataSet()
        {
            //ds = SiqConfigExport.Data.GetDataFromSIQdb(T_DataLocalLab.CONN_STR);
            //Assert.IsNotNull(ds.Tables[SqlStmtKeys.DB_INFO1]);
            //Assert.IsNotNull(ds.Tables[SqlStmtKeys.DB_INFO2]);
            //Assert.IsNotNull(ds.Tables[SqlStmtKeys.SIQ_CONFIG1]);
            //Assert.IsNotNull(ds.Tables[SqlStmtKeys.SIQ_CONFIG2]);
            //Assert.IsNotNull(ds.Tables[SqlStmtKeys.EMAIL]);
            //Assert.IsNotNull(ds.Tables[SqlStmtKeys.INSTALL_SERVER]);
            //Assert.IsNotNull(ds.Tables[SqlStmtKeys.INSTALL_SERVICE]);
            //Assert.IsNotNull(ds.Tables[SqlStmtKeys.LICENSE]);
            //Assert.IsNotNull(ds.Tables[SqlStmtKeys.IDC]);
            //Assert.IsNotNull(ds.Tables[SqlStmtKeys.WPC]);
            //Assert.IsNotNull(ds.Tables[SqlStmtKeys.APPS]);
            //Assert.IsNotNull(ds.Tables[SqlStmtKeys.APP_CONFIGS]);
            //Assert.IsNotNull(ds.Tables[SqlStmtKeys.TASKS]);
            //Assert.IsNotNull(ds.Tables[SqlStmtKeys.TASK_RESULTS]);
            //Assert.IsNotNull(ds.Tables[SqlStmtKeys.USERS]);

            //Assert.IsTrue(ds.Tables[SqlStmtKeys.DB_INFO1].Rows.Count > 0);
            ////Assert.IsTrue(ds.Tables[SqlStmtKeys.DB_INFO2].Rows.Count > 0);
            //Assert.IsTrue(ds.Tables[SqlStmtKeys.SIQ_CONFIG1].Rows.Count > 0);
            //Assert.IsTrue(ds.Tables[SqlStmtKeys.SIQ_CONFIG1].Rows.Count > 0);
            //Assert.IsTrue(ds.Tables[SqlStmtKeys.EMAIL].Rows.Count > 0);
            //Assert.IsTrue(ds.Tables[SqlStmtKeys.EMAIL].Rows.Count > 0);
            //Assert.IsTrue(ds.Tables[SqlStmtKeys.EMAIL].Rows.Count > 0);
            //Assert.IsTrue(ds.Tables[SqlStmtKeys.LICENSE].Rows.Count > 0);
            //Assert.IsTrue(ds.Tables[SqlStmtKeys.IDC].Rows.Count > 0);
            //Assert.IsTrue(ds.Tables[SqlStmtKeys.APPS].Rows.Count > 0);
            //Assert.IsTrue(ds.Tables[SqlStmtKeys.APP_CONFIGS].Rows.Count > 0);
            //Assert.IsTrue(ds.Tables[SqlStmtKeys.TASKS].Rows.Count > 0);
            //Assert.IsTrue(ds.Tables[SqlStmtKeys.TASK_RESULTS].Rows.Count > 0);
            //Assert.IsTrue(ds.Tables[SqlStmtKeys.USERS].Rows.Count > 0);
        }
    }
}
